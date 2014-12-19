using System;
namespace FileAdmiral.Engine.ViewModels
{
    interface ICommandShellViewModel
    {
        string FolderPath { get; }
        string Prompt { get; }
        event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        void SendCommand(string command, bool store = true);
        string StandardOut { get; }
    }
}
