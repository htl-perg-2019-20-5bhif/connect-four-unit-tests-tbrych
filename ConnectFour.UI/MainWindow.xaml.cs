using System.Windows;
using System.Windows.Controls;

namespace ConnectFour.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel vm = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
            vm.Init();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.SetStone(int.Parse(((Button)sender).Tag.ToString()));
        }
    }
}
