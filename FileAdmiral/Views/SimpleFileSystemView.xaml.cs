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
    /// Interaction logic for SimpleFileSystemView.xaml
    /// </summary>
    public partial class SimpleFileSystemView : UserControl, IFileSystemView
    {
        private IMainViewModel _mainViewModel;
        private IFileSystemViewModel _fileItems;
        public SimpleFileSystemView(IMainViewModel viewModel)
        {
            InitializeComponent();
            _mainViewModel = viewModel;
            //_fileItems = fileItems;
            //DataContext = _fileItems;
        }


    }
}
