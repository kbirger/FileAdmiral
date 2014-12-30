using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FileAdmiral.Engine.ViewModels
{
    public interface IFileSystemViewModel
    {
        string ViewType { get; set; }
        string CurrentPath { get; set; }
        ReadOnlyObservableCollection<FolderItem> Items { get; }
        ICommand ChangeDirectoryCommand { get; }

        void Initialize(string startPath);
    }
}