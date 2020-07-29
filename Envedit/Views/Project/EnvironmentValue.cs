using System.Text.Json.Serialization;
using System.Windows.Input;
using Oniqys.Wpf.MVVM;

namespace Envedit
{
    public class EnvironmentValue : ViewModelBase
    {
        private string _value;

        public string Value
        {
            get => _value;
            set => UpdateClassField(ref _value, value);
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { UpdateClassField(ref _description, value); }
        }

        [JsonIgnore]
        public ICommand UpdateCommand { get; set; }
    }
}
