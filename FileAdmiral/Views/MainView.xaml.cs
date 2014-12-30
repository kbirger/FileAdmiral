using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FileAdmiral.Engine.ViewModels;
using FileAdmiral.Engine.Views;

namespace FileAdmiral.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl, IMainView
    {
        private IMainViewModel _viewModel;
        private IViewFactory _viewFactory;
        public MainView(IMainViewModel viewModel, IViewFactory viewFactory)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _viewFactory = viewFactory;
            Loaded += MainView_Loaded;
        }

        void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            RightItemContainer.DataContext = _viewModel.RightItems;
            RightItemContainer.Content = _viewFactory.CreateView<IFileSystemView>();
         
            LeftItemContainer.DataContext = _viewModel.LeftItems;
            LeftItemContainer.Content = _viewFactory.CreateView<IFileSystemView>();

            CommandShellContainer.DataContext = _viewModel.CommandShell;
            CommandShellContainer.Content = _viewFactory.CreateView<ICommandShellView>();
        }
    }
}
