using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Oniqys.Core;

namespace Envedit
{
    /// <summary>
    /// プロジェクトを選択する機能です。
    /// </summary>
    public class ProjectSelectorModel
    {
        event EventHandler<EventArgs<ProjectModel>> CurrentProjectChanged;

        private ProjectModel _currentProject;

        private Configuration _configuration;

        public ProjectModel CurrentProject
        {
            get => _currentProject;
            set
            {
                if (_currentProject == value)
                    return;

                _currentProject = value;
                CurrentProjectChanged?.Invoke(this, new EventArgs<ProjectModel>(_currentProject));
            }
        }

        public List<string> RecentProjects { get; }

        public ProjectSelectorModel()
        {
            _configuration = ConfigurationManager.OpenExeConfiguration(
                    ConfigurationUserLevel.None);

            var recentProjects = _configuration.AppSettings.Settings["RecentProjects"];

            RecentProjects = recentProjects.Value.Split(';').ToList();
        }

        public void SaveSettings()
        {
            _configuration.Save();
        }
    }
}
