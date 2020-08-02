namespace Oniqys.Wpf.MVVM
{
    public class NavigationViewModelBase : ViewModelBase
    {
        private NavigationViewModelBase _nextView;

        public NavigationViewModelBase NextView
        {
            get { return _nextView; }
            set { UpdateClassField(ref _nextView, value); }
        }

        private NavigationViewModelBase _previousView;

        public NavigationViewModelBase PreviousView
        {
            get { return _previousView; }
            set { UpdateClassField(ref _previousView, value); }
        }

    }
}
