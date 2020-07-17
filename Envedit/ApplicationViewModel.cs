using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Oniqys.Wpf;
using Oniqys.Wpf.MVVM;

namespace Envedit
{
    public class ApplicationViewModel : ViewModelBase
    {
        private readonly ApplicationModel _model = new ApplicationModel();

        private readonly IClosable _closer;

        private bool _isLocalDB;

        public bool IsLocalDB
        {
            get => _isLocalDB;
            set => UpdateStructField(ref _isLocalDB, value);
        }

        private bool _isDockerDB;

        public bool IsDockerDB
        {
            get => _isDockerDB;
            set => UpdateStructField(ref _isDockerDB, value);
        }

        private bool _isDevelopDB;

        public bool IsDevelopDB
        {
            get => _isDevelopDB;
            set => UpdateStructField(ref _isDevelopDB, value);
        }

        private bool _isDebugService;

        public bool IsDebugService
        {
            get => _isDebugService;
            set => UpdateStructField(ref _isDebugService, value);
        }

        private bool _isDockerService;

        public bool IsDockerService
        {
            get => _isDockerService;
            set => UpdateStructField(ref _isDockerService, value);
        }

        private bool _isDevelopService;

        public bool IsDevelopService
        {
            get => _isDevelopService;
            set => UpdateStructField(ref _isDevelopService, value);
        }

        private bool _isDebugFrontend;

        public bool IsDebugFrontend
        {
            get => _isDebugFrontend;
            set => UpdateStructField(ref _isDebugFrontend, value);
        }

        private bool _isDockerFrontend;

        public bool IsDockerFrontend
        {
            get => _isDockerFrontend;
            set => UpdateStructField(ref _isDockerFrontend, value);
        }

        private bool _isDevelopFrontend;

        public bool IsDevelopFrontend
        {
            get => _isDevelopFrontend;
            set => UpdateStructField(ref _isDevelopFrontend, value);
        }
        public ObservableCollection<EnvironmentValue> Values { get; } = new ObservableCollection<EnvironmentValue>();

        public ObservableCollection<EnvironmentValuePath> Pathes { get; } = new ObservableCollection<EnvironmentValuePath>();

        private EnvironmentValue _selectedItem;

        public EnvironmentValue SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (UpdateClassField(ref _selectedItem, value) && _selectedItem != null)
                {
                    foreach (var path in Pathes)
                    {
                        path.Selected = path.Value == _selectedItem.Value;
                    }
                }
            }
        }

        public ICommand ClearCommand { get; }

        public ICommand RefreshValuesCommand { get; }

        public ICommand LoadEnvTemplateValueCommand { get; }

        public ICommand LoadEnvValueCommand { get; }

        public ICommand CreateRelationCommand { get; }

        public ICommand CloseCommand { get; }

        public ICommand UpdateComand { get; }

        public ICommand SaveToEnvironmentCommand { get; }

        public ICommand SaveTemplateCommand { get; }

        public ICommand LoadTemplateCommand { get; }


        public ApplicationViewModel()
        {
            _model.PathesChanged += OnModelPathesChanged;
            _model.ValuesChanged += OnModelValuesChanged;

            ClearCommand = new DelegateCommand(() => { _model.Clear(); });

            RefreshValuesCommand = new DelegateCommand(() => { _model.ReadCurrentEnvironment(); });

            LoadEnvTemplateValueCommand = new DelegateCommand(() => _model.LoadEnvTemplate());
            LoadEnvValueCommand = new DelegateCommand(() => { _model.LoadEnvValues(); });

            CloseCommand = new DelegateCommand(() => Close());
            UpdateComand = new DelegateCommand<string>(UpdateValues);

            SaveToEnvironmentCommand = new DelegateCommand(() => _model.SaveEnvironment());

            LoadTemplateCommand = new DelegateCommand(() => { SaveFlags(); _model.LoadTemplate(); });
            SaveTemplateCommand = new DelegateCommand(() => { SaveFlags(); _model.SaveTemplate(); });

            SetupFlags();

            RefleshValues();
            RefreshPathes();
        }

        private void SetupFlags()
        {
            IsLocalDB = _model.Configuration.SelectedDB == 0;
            IsDockerDB = _model.Configuration.SelectedDB == 1;
            IsDevelopDB = _model.Configuration.SelectedDB == 2;

            IsDebugService = _model.Configuration.SelectedService == 0;
            IsDockerService = _model.Configuration.SelectedService == 1;
            IsDevelopService = _model.Configuration.SelectedService == 2;

            IsDebugFrontend = _model.Configuration.SelectedClient == 0;
            IsDockerFrontend = _model.Configuration.SelectedClient == 1;
            IsDevelopFrontend = _model.Configuration.SelectedClient == 2;

        }

        private void SaveFlags()
        {
            _model.Configuration.SelectedDB = IsLocalDB ? 0 : IsDockerDB ? 1 : IsDevelopDB ? 2 : 0;
            _model.Configuration.SelectedService = IsDebugService ? 0 : IsDockerService ? 1 : IsDevelopService ? 2 : 0;
            _model.Configuration.SelectedClient = IsDebugFrontend ? 0 : IsDockerFrontend ? 1 : IsDevelopFrontend ? 2 : 0;
        }


        private void UpdateValues(string param)
        {
            foreach (var path in Pathes.Where(each => each.Selected))
            {
                path.Value = param;
            }
        }

        public ApplicationViewModel(IClosable closer)
            : this()
        {
            _closer = closer;
        }

        private void Close()
        {
            SaveFlags();
            _model.WriteSettings();
            _closer?.Close();
        }

        private void OnModelValuesChanged(object sender, System.EventArgs e) => RefleshValues();

        private void RefleshValues()
        {
            Values.Clear();
            foreach (var value in _model.Values)
            {
                value.UpdateCommand = UpdateComand;
                Values.Add(value);
            }
        }

        private void OnModelPathesChanged(object sender, System.EventArgs e) => RefreshPathes();
        private void RefreshPathes()
        {
            Pathes.Clear();
            foreach (var path in _model.Pathes)
            {
                Pathes.Add(path);
            }
        }
    }
}
