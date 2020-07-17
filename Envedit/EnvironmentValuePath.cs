using Oniqys.Wpf.MVVM;

namespace Envedit
{
    public class EnvironmentValuePath : ViewModelBase
    {
        private string _path;

        public string Path
        {
            get { return _path; }
            set { UpdateClassField(ref _path, value); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { UpdateClassField(ref _name, value); }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set { UpdateClassField(ref _value, value); }
        }

        private bool _selected;

        public bool Selected
        {
            get { return _selected; }
            set { UpdateStructField(ref _selected, value); ; }
        }

    }
}
