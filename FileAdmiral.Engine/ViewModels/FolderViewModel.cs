using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FileAdmiral.Engine.Annotations;

namespace FileAdmiral.Engine.ViewModels
{
    public class FolderViewModel : INotifyPropertyChanged, IFileSystemViewModel
    {
        private string _viewType;
        private string _currentPath;
        private ObservableCollection<FolderItem> _items = new ObservableCollection<FolderItem>();
        private ReadOnlyObservableCollection<FolderItem> _readOnlyItems;
        private DelegateCommand _changeDirectoryCommand;
        private DelegateCommand _deleteItemCommand;
        private DelegateCommand _addItemCommand;

        public FolderViewModel()
        {
            _readOnlyItems = new ReadOnlyObservableCollection<FolderItem>(_items);
            _changeDirectoryCommand = new DelegateCommand(o => CurrentPath = ((FolderItem)o).ResourcePath);
            _deleteItemCommand = new DelegateCommand(o => File.Delete(((FolderItem)o).ResourcePath));
            _addItemCommand = new DelegateCommand(o => File.Create(((FolderItem)o).ResourcePath));
        }

        public string ViewType
        {
            get { return _viewType; }
            set
            {
                if (_viewType != value)
                {
                    _viewType = value;
                    OnPropertyChanged("ViewType");
                }
            }
        }

        public string CurrentPath
        {
            get { return _currentPath; }
            set
            {
                if (_currentPath != value)
                {
                    _currentPath = value;
                    OnPropertyChanged("CurrentPath");
                    LoadItems();
                }
            }
        }

        public ReadOnlyObservableCollection<FolderItem> Items
        {
            get { return _readOnlyItems; }
        }

        public ICommand ChangeDirectoryCommand
        {
            get { return _changeDirectoryCommand; }
        }
        

        private void LoadItems()
        {
            _items.Clear();
            var directory = new DirectoryInfo(CurrentPath);
            foreach (var item in directory.EnumerateFileSystemInfos())
            {
                _items.Add(new FolderItem(item.Name, item.FullName, !item.Attributes.HasFlag(FileAttributes.Directory)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


        public void Initialize(string startPath)
        {
            CurrentPath = startPath;
        }
    }
}
