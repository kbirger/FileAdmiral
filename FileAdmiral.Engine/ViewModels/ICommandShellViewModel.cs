using System;
using System.ComponentModel;

namespace FileAdmiral.Engine.ViewModels
{
    public interface ICommandShellViewModel : INotifyPropertyChanged, IDisposable
    {
        string Prompt { get; }
        string FolderPath { get; }
        string StandardOut { get; }
        void SendCommand(string command);
        event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void Dispose();
    }
}