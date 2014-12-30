using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAdmiral.Engine.ViewModels
{
    public class MainViewModel
    {
        private ICommandShellViewModel _commandShell = new PowerShellViewModel("C:\\");
        private FolderViewModel _leftItems = new FolderViewModel("C:\\");
        private FolderViewModel _rightItems = new FolderViewModel("C:\\");

        public MainViewModel()
        {
            
        }

        public ICommandShellViewModel CommandShell
        {
            get { return _commandShell; }
        }

        public FolderViewModel LeftItems
        {
            get { return _leftItems; }
        }

        public FolderViewModel RightItems
        {
            get { return _rightItems; }
        }
    }
}
