using Oniqys.Wpf.MVVM;

namespace Envedit
{
    public class EnveditModel : ViewModelBase
    {
        private ProjectModel _currentProject;

        public ProjectModel CurrentProject
        {
            get => _currentProject;
            set => UpdateClassField(ref _currentProject, value);
        }


    }
}
