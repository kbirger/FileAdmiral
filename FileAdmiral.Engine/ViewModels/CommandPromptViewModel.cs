using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FileAdmiral.Engine.Annotations;

namespace FileAdmiral.Engine.ViewModels
{
    public class CommandPromptViewModel : ICommandShellViewModel
    {
        private const int CAPACITY = 200;
        private readonly Process _process;
        private readonly CircularBuffer _stdOut = new CircularBuffer(CAPACITY); 
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
            _process.Start();
            _process.BeginErrorReadLine();
            // 'mode con:cols=920 lines=40'
            SendCommand("set PROMPT=" + PROMPT_MAGIC +"$P$G::");
            Task.Run(() => Read());

        }

        

        private const string PROMPT_MAGIC = "PROMPT::";

        private void Read()
        {
            var buffer = new StringBuilder();
            while (!_process.HasExited)
            {
                int readOut = _process.StandardOutput.Read();
                if (readOut == -1)
                {
                    throw new InvalidOperationException("unexpected end of stdout");
                    break;
                }
                char readChar = (char) readOut;

                
                if (readChar == '\n')
                {
                    string bufferString = buffer.ToString().Trim();
                    if (!bufferString.Contains(PROMPT_MAGIC))
                    {
                        _stdOut.AddLine(bufferString);
                        OnPropertyChanged("StandardOut");
                    }
                    buffer.Clear();
                }
                else if (readChar == '\r')
                {

                }
                else if (readChar != '\0')
                {
                    buffer.Append(readChar);
                    string lastLineClean = buffer.ToString().Trim();
                    int indexOfPathMagic = lastLineClean.IndexOf(PROMPT_MAGIC, StringComparison.CurrentCulture);
                    if (indexOfPathMagic > -1)
                    {
                        int indexOfPathMagicEnd = lastLineClean.IndexOf(">::", StringComparison.CurrentCulture);
                        if (indexOfPathMagicEnd > -1)
                        {
                            int start = indexOfPathMagic + PROMPT_MAGIC.Length;
                            int end = lastLineClean.LastIndexOf("::", StringComparison.CurrentCulture);
                            Prompt = lastLineClean.Substring(start, end - start);
                            FolderPath = Prompt.Substring(0, Prompt.Length - 1);
                        }
                    }
                }
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
                    OnPropertyChanged("Prompt");
                }
            }
        }

        public void SendCommand(string command)
        {
            _process.StandardInput.WriteLine(command);
        }

        public string FolderPath { get; private set; }

        public string StandardOut
        {
            get { return _stdOut.ToString(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
            if (_process != null && !_process.HasExited)
            {
                _process.Kill();
            }
        }

        ~CommandPromptViewModel()
        {
            Dispose();
        }
    }
}
