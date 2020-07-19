using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Envedit
{
    class ProjectModel
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public event EventHandler ValuesChanged;

        public event EventHandler PathesChanged;

        public ObservableCollection<EnvironmentValue> Values { get; } = new ObservableCollection<EnvironmentValue>();

        public ObservableCollection<EnvironmentValuePath> Pathes { get; } = new ObservableCollection<EnvironmentValuePath>();

        public ProjectModel()
        {
            Configuration = ReadSettings();

            ReadCurrentEnvironment();

            LoadTemplate();
        }

        public class Config
        {
            public int SelectedDB { get; set; }

            public int SelectedClient { get; set; }

            public int SelectedService { get; set; }
        }

        public Config Configuration { get; set; }

        private Config ReadSettings()
        {
            try
            {
                return JsonSerializer.Deserialize<Config>(File.ReadAllText("application.settings"), _options) ?? new Config();
            }
            catch
            {
                return new Config();
            }
        }

        public void WriteSettings()
        {
            var json = JsonSerializer.Serialize<Config>(Configuration, _options);
            File.WriteAllText("application.settings", json);
        }

        public void Clear()
        {
            Values.Clear();
            ValuesChanged?.Invoke(this, EventArgs.Empty);

            Pathes.Clear();
            PathesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void LoadEnvTemplate()
        {
            Pathes.Clear();
            LoadValueAndPathes(".env.template", ".template");
            PathesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void LoadEnvValues()
        {
            Pathes.Clear();
            LoadValueAndPathes(".env");
            PathesChanged?.Invoke(this, EventArgs.Empty);
        }

        private void LoadValueAndPathes(string filename, string trim = null)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var templates = Directory.GetFiles(currentDirectory, filename, SearchOption.AllDirectories);

            foreach (var template in templates)
            {
                var lines = File.ReadAllLines(template);

                var relativePath = Path.GetRelativePath(currentDirectory, template);

                if (!string.IsNullOrEmpty(trim))
                    relativePath = relativePath.Substring(0, relativePath.Length - trim.Length);

                var valueAndPathes = Parse(lines);

                foreach (var path in valueAndPathes.Select(each => new EnvironmentValuePath { Path = relativePath, Name = each.name, Value = each.value }))
                {
                    Pathes.Add(path);
                }

                foreach (var value in valueAndPathes)
                {
                    if (!Values.Any(each => each.Value == value.value))
                        Values.Add(new EnvironmentValue { Value = value.value, Description = value.name });
                }

            }

            var valueDic = Values.Distinct().ToDictionary(each => each.Value);

            ValuesChanged?.Invoke(this, EventArgs.Empty);
            PathesChanged?.Invoke(this, EventArgs.Empty);
        }

        public class ConfigItem { public string Value { get; set; } public string Description { get; set; } }

        public void ReadCurrentEnvironment()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var configPath = Path.Combine(currentDirectory, "envmanager.json");

            if (File.Exists(configPath))
            {
                var configString = File.ReadAllText(configPath);

                var configs = JsonSerializer.Deserialize<ConfigItem[]>(configString, _options);

                foreach (var config in configs)
                {
                    Values.Add(new EnvironmentValue { Value = config.Value, Description = config.Description });
                }
                ValuesChanged?.Invoke(this, EventArgs.Empty);
            }

        }

        public void SaveEnvironment()
        {
            var groupedPathes = Pathes.GroupBy(each => each.Path);

            foreach (var group in groupedPathes)
            {
                File.WriteAllLines(group.Key, group.Select(each => $"{each.Name}={each.Value}"));
            }
        }

        private IEnumerable<(string name, string value)> Parse(string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.Length <= 2 || line[0] == '#')
                    continue;

                string[] array = line.Split(new[] { '=' }, 2);
                if (array.Length == 2)
                {
                    array[0] = array[0].Trim();
                    array[1] = array[1].Trim();

                    yield return (array[0], array[1]);
                }
            }
        }

        private string GetTemplateFilename()
        {
            return $"{Configuration.SelectedDB}-{Configuration.SelectedService}-{Configuration.SelectedClient}.templateset";
        }

        public void SaveTemplate()
        {
            var filename = GetTemplateFilename();

            var template = new Template
            {
                Pathes = Pathes.ToArray(),
                Values = Values.ToArray(),
            };

            var json = JsonSerializer.Serialize<Template>(template, _options);

            File.Delete(filename);
            File.WriteAllText(filename, json);
        }

        public void LoadTemplate()
        {
            var filename = GetTemplateFilename();

            try
            {
                var template = JsonSerializer.Deserialize<Template>(File.ReadAllText(filename), _options);

                Pathes.Clear();

                foreach (var path in template.Pathes)
                {
                    Pathes.Add(path);
                }

                foreach (var value in template.Values)
                {
                    if (!Values.Any(each => each.Value == value.Value))
                        Values.Add(value);
                }
                ValuesChanged?.Invoke(this, EventArgs.Empty);
                PathesChanged?.Invoke(this, EventArgs.Empty);
            }
            catch
            {
                LoadEnvValues();
            }

        }
    }
}
