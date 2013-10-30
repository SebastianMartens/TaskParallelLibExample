using System.Windows;

namespace WpfApplication1.UI
{
    /// <summary>
    ///     Interaction logic for WPFDataAccessAsync.xaml
    ///     
    /// </summary>
    public partial class MainWindow
    {        
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

            InitializeComponent();
        }         

        private void C_Ende_Click(object sender, RoutedEventArgs e)
        {
            Close(); // in real-world apps we would have a command here, too...
        }
            
     
    }
}