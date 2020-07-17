using System;
using System.Windows;
using Oniqys.Wpf;

namespace Envedit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IClosable

    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ApplicationViewModel(this);
        }
    }
}
