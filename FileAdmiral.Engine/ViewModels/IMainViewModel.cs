using System;
namespace FileAdmiral.Engine.ViewModels
{
    public interface IMainViewModel
    {
        ICommandShellViewModel CommandShell { get; }
        void Initialize(string leftUri, string rightUri);
        IFileSystemViewModel LeftItems { get; }
        IFileSystemViewModel RightItems { get; }
    }
}
