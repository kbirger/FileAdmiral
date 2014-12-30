using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAdmiral.Engine.ViewModels
{
    public class MainViewModel : IMainViewModel
    {
        private ICommandShellViewModel _commandShell;
        private IFileSystemViewModelFactory _viewModelFactory;
        private IFileSystemViewModel _leftItems;
        private IFileSystemViewModel _rightItems;

        public MainViewModel(IFileSystemViewModelFactory viewModelFactory, ICommandShellViewModel commandShellViewModel)
        {
            _viewModelFactory = viewModelFactory;
            _commandShell = commandShellViewModel;
            
        }

        public void Initialize(string leftUri, string rightUri)
        {
            _leftItems = _viewModelFactory.CreateViewModel(leftUri);
            _rightItems = _viewModelFactory.CreateViewModel(rightUri);
            _commandShell.FolderPath = _leftItems.CurrentPath;
        }

        public ICommandShellViewModel CommandShell
        {
            get { return _commandShell; }
        }

        public IFileSystemViewModel LeftItems
        {
            get { return _leftItems; }
        }

        public IFileSystemViewModel RightItems
        {
            get { return _rightItems; }
        }
    }
}
