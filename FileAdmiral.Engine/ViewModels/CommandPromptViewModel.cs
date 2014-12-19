using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FileAdmiral.Engine.Annotations;
using System.Runtime.InteropServices;
using ReadEnvSample;

namespace FileAdmiral.Engine.ViewModels
{
    public class CommandPromptViewModel : INotifyPropertyChanged, FileAdmiral.Engine.ViewModels.ICommandShellViewModel
    {
        private const int CAPACITY = 200;
        private readonly Process _process;
        private readonly Queue<string> _stdOut = new Queue<string>(CAPACITY); 
        public CommandPromptViewModel(string folderPath)
        {
            FolderPath = folderPath;
            _process = new Process
            {
                StartInfo = new ProcessStartInfo("cmd")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    WorkingDirectory = folderPath
                }
            };
            _process.OutputDataReceived += ProcessOnOutputDataReceived;
            _process.Start();
            _process.BeginOutputReadLine();

            UpdatePrompt();


        }
        private void UpdatePrompt()
        {
            _process.CancelOutputRead();
            SendCommand("set FA_CD=%CD%");
            _process.BeginOutputReadLine();



            var envars = _process.TryReadEnvironmentVariables();
            if(envars!=null)
                Prompt = envars["FA_CD"] + ">";

        }
        private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            string data = e.Data;
            UpdatePrompt();
            

            if (_stdOut.Count == CAPACITY)
            {
                _stdOut.Dequeue();
            }
            _stdOut.Enqueue(data);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("StandardOut"));
            }
        }

        private string _prompt;

        public string Prompt
        {
            get { return _prompt; }
            private set
            {
                if (_prompt != value)
                {
                    _prompt = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Prompt"));
                    }
                }
            }
        }

        private string _lastCommand;
        private bool _lastInputWasMine;
        public void SendCommand(string command, bool store = true)
        {
            if (store)
            {
                _lastCommand = command;
            }
            _lastInputWasMine = true;
            _process.StandardInput.WriteLine(command);
        }

        private const string SETPATH = "set FA_CD=%CD%";
        public string FolderPath { get; private set; }

        //private StringBuilder _stdOut = new StringBuilder();

        public string StandardOut
        {
            get { return string.Join("\n",_stdOut); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
