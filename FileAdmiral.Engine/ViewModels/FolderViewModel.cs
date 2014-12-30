using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FileAdmiral.Engine.Annotations;
using System.Collections.Generic;
using System;

namespace FileAdmiral.Engine.ViewModels
{
    public class FolderViewModel : INotifyPropertyChanged, IFileSystemViewModel
    {
        private class PathStack
        {
            private Stack<string> _pathParts;
            public PathStack(string path)
            {
                //_pathParts = new Stack<string>();
                //// get rid of trailing slash for now

                //path = path.TrimEnd('\\');

                //int partEnd = path.Length - 1;
                //for (int i = path.Length - 1; i >= 0; i--)
                //{
                //    var c = path[i];
                //    if (partEnd == -1)
                //    {
                //        partEnd = i;
                //    }
                //    if (c == '\\')
                //    {
                //        if (partEnd > i)
                //        {
                //            _pathParts.Push(path.Substring(i + 1, partEnd - i));
                //        }
                //        partEnd = i - 1;
                //    }
                    
                //}
                _pathParts = new Stack<string>(path.Split(new [] {'\\'}, StringSplitOptions.RemoveEmptyEntries));
            }
            public bool IsRoot
            {
                get
                {
                    return _pathParts.Count == 1;
                }
            }
            public string GoUp()
            {
                _pathParts.Pop();
                return ToString();
            }
            public string AddDirectory(string folder)
            {
                _pathParts.Push(folder);
                return ToString();
            }
            public override string ToString()
            {
                if (!IsRoot)
                {
                    return string.Join("\\", _pathParts.Reverse());
                }
                else
                {
                    // C# will resolve "C:" as a relative path, which is not what we want.
                    // todo: UNC support will break this logic, but that's a problem for Future Me.
                    return _pathParts.Peek() + "\\";
                }
            }
        }
        private string _viewType;
        private string _currentPath;
        private ObservableCollection<FolderItem> _items = new ObservableCollection<FolderItem>();
        private ReadOnlyObservableCollection<FolderItem> _readOnlyItems;
        private DelegateCommand _changeDirectoryCommand;
        private DelegateCommand _deleteItemCommand;
        private DelegateCommand _addItemCommand;
        private DelegateCommand _goUpCommand;

        public FolderViewModel()
        {
            _readOnlyItems = new ReadOnlyObservableCollection<FolderItem>(_items);
            _changeDirectoryCommand = new DelegateCommand(o => CurrentPath = ((FolderItem)o).ResourcePath);
            _deleteItemCommand = new DelegateCommand(o => File.Delete(((FolderItem)o).ResourcePath));
            _addItemCommand = new DelegateCommand(o => File.Create(((FolderItem)o).ResourcePath));
            _goUpCommand = new DelegateCommand(o => CurrentPath = new PathStack(CurrentPath).GoUp(), o => !(new PathStack(CurrentPath).IsRoot));
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
                    if (value.StartsWith("file:///"))
                    {
                        var uri = new Uri(value);
                        value = uri.LocalPath;
                    }

                    // expand env vars
                    _currentPath = new DirectoryInfo(value).FullName;
                    _goUpCommand.RaiseCanExecuteChanged();
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

        public ICommand GoUpCommand
        {
            get { return _goUpCommand; }
        }

        private void LoadItems()
        {
            _items.Clear();
            var directory = new DirectoryInfo(CurrentPath);
            var directoryNavigator = new PathStack(CurrentPath);
            if(!directoryNavigator.IsRoot)
            {
                _items.Add(new FolderItem("..", directoryNavigator.GoUp(), false));
            }
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
