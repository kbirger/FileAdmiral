﻿using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;
using FileAdmiral.Engine.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FileAdmiral.Engine.ViewModels
{
    public class PowerShellViewModel : ICommandShellViewModel
    {
        private PowerShell _shell;
        private CircularBuffer _buffer = new CircularBuffer(200);
        private string _folderPath;
        private PSHost _psHost;
        public PowerShellViewModel(string folderPath)
        {
            _buffer.Changed += (sender, args) => OnPropertyChanged("StandardOut");
            _shell = PowerShell.Create();
            _psHost = new MyPSHost(_buffer);
            _shell.Runspace = RunspaceFactory.CreateRunspace(_psHost);
            _shell.Runspace.Open();
            FolderPath = folderPath;

        }
        public string FolderPath
        {
            get
            {
                return _folderPath;
            }
            set
            {
                if (_folderPath != value)
                {
                    _shell.AddScript("cd " + value).Invoke();
                    _folderPath = GetPWD();
                    Prompt = _folderPath + ">";
                    OnPropertyChanged("FolderPath");
                }
            }
        }

        private string GetPWD()
        {
            using (Pipeline pipeline = _shell.Runspace.CreatePipeline())
            {
                pipeline.Commands.AddScript("pwd");
                //pipeline.Commands[0].MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);
                //pipeline.Commands.Add("out-default");

                return pipeline.Invoke().Single().ToString();
            }
        }
        private string _prompt;
        public string Prompt
        {
            get
            {
                return _prompt;
            }
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


        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public void SendCommand(string command)
        {

            //using (Runspace runspace = RunspaceFactory.CreateRunspace(host))
            {
                //runspace.Open();
                using (Pipeline pipeline = _shell.Runspace.CreatePipeline())
                {
                    
                    _shell.Streams.ClearStreams();
                    pipeline.Commands.AddScript(command);
                    pipeline.Commands[0].MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);
                    pipeline.Commands.Add("out-default");

                    pipeline.Invoke();
                }
            }
            Prompt = GetPWD() + ">";

            //_buffer.AddLines(((MyPSHostUserInterface)_psHost.UI).Output.Split(new string[] { "\r\n" }, StringSplitOptions.None));
            //OnPropertyChanged("StandardOut");
        }
        public void SendCommand2(string command)
        {
            //using (var pipeline = _shell.Runspace.CreatePipeline("Out-String"))
            //{
            //    pipeline.Commands.AddScript(command);
            //    //pipeline.Commands[0].MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);
            //    //var res = pipeline.Invoke();
            //    //_buffer.AddLines(res.SelectMany(r => r.Split(new string[] { "\r\n"}, StringSplitOptions.None))););
            //}
            _shell.Commands.Clear();
            _shell.Streams.ClearStreams();
            _shell.AddScript(command);
            _shell.Commands.Commands[0].MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);
            _shell.AddCommand("Out-String");

            _buffer.AddLines(_shell.Invoke<string>().SelectMany(r => r.Split(new string[] { "\r\n" }, StringSplitOptions.None)));
            _buffer.AddLines(_shell.Streams.Error.ReadAll().Select(e => e.Exception.Message));
            OnPropertyChanged("StandardOut");
            Prompt = GetPWD() + ">";
        }

        public string StandardOut
        {
            get { return string.Join("\n", _buffer); }
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _shell.Dispose();
        }

        ~PowerShellViewModel()
        {
            Dispose();
        }
    }
}
