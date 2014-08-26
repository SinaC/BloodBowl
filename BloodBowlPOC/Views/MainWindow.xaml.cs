using System.Windows;
using BloodBowlPOC.ViewModels;

namespace BloodBowlPOC.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainViewModel vm = new MainViewModel();
            DataContext = vm;

            vm.Initialize();
        }
    }
}