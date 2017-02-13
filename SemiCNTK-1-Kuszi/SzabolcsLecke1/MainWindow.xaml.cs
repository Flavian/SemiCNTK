using System.Windows;

namespace SzabolcsLecke1
{
    public partial class MainWindow
    {
        #region Declarations

        private readonly ViewModel _viewModel;

        #endregion

        // ********************************************************************

        #region Constructor

        public MainWindow()
        {
            _viewModel = new ViewModel();

            InitializeComponent();

            DataContext = _viewModel;
        }

        #endregion

        // ********************************************************************

        #region Init

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Init();
        } 

        #endregion
    }
}
