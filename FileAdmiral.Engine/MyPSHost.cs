using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileAdmiral.Engine
{
    public class MyPSHost : PSHost
    {
        private Guid _hostId = Guid.NewGuid();
        private MyPSHostUserInterface _ui;

        public MyPSHost(CircularBuffer buffer)
        {
            _ui = new MyPSHostUserInterface(buffer);
        }

        public override void SetShouldExit(int exitCode)
        {
            return;
        }

        public override void EnterNestedPrompt()
        {
            throw new NotImplementedException();
        }

        public override void ExitNestedPrompt()
        {
            throw new NotImplementedException();
        }

        public override void NotifyBeginApplication()
        {
            throw new NotImplementedException();
        }

        public override void NotifyEndApplication()
        {
            throw new NotImplementedException();
        }

        public override string Name
        {
            get { return "MyHost"; }
        }

        public override Version Version
        {
            get { return new Version(2, 0); }
        }

        public override Guid InstanceId
        {
            get { return _hostId; }
        }

        public override PSHostUserInterface UI
        {
            get { return _ui; }
        }

        public override CultureInfo CurrentCulture
        {
            get { return Thread.CurrentThread.CurrentCulture; }
        }

        public override CultureInfo CurrentUICulture
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
        }
    }




    internal class MyPSHostUserInterface : PSHostUserInterface
    {
        private CircularBuffer _buffer;

        public MyPSHostUserInterface(CircularBuffer buffer)
        {
            _buffer = buffer;
            //_myRawUi.WindowSize = new Size(9999, 9999);
        }

        public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
        {
            _buffer.AddLine(value);
        }

        public override void Write(string value)
        {
            _buffer.AddLine(value);
        }

        public override void WriteDebugLine(string message)
        {
            _buffer.AddLine("DEBUG: " + message);
        }

        public override void WriteErrorLine(string value)
        {
            _buffer.AddLine("ERROR: " + value);
        }

        public override void WriteLine(string value)
        {
            _buffer.AddLine(value);
        }

        public override void WriteVerboseLine(string message)
        {
            _buffer.AddLine("VERBOSE: " + message);
        }

        public override void WriteWarningLine(string message)
        {
            _buffer.AddLine("WARNING: " + message);
        }

        public override void WriteProgress(long sourceId, ProgressRecord record)
        {
            return;
        }

        
        public override Dictionary<string, PSObject> Prompt(string caption, string message, System.Collections.ObjectModel.Collection<FieldDescription> descriptions)
        {
            throw new NotImplementedException();
        }

        public override int PromptForChoice(string caption, string message, System.Collections.ObjectModel.Collection<ChoiceDescription> choices, int defaultChoice)
        {
            throw new NotImplementedException();
        }

        public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options)
        {
            throw new NotImplementedException();
        }

        public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName)
        {
            throw new NotImplementedException();
        }

        private MyRawUI _myRawUi = new MyRawUI();
        public override PSHostRawUserInterface RawUI
        {
            get { return _myRawUi; }
        }

        public override string ReadLine()
        {
            throw new NotImplementedException();
        }

        public override System.Security.SecureString ReadLineAsSecureString()
        {
            throw new NotImplementedException();
        }
    }

    public class MyRawUI : PSHostRawUserInterface
    {
        private bool _keyAvailable;
        private Size _maxSize;

        public MyRawUI()
        {
            BufferSize = new Size(200, 200);
            CursorSize = 25;
            _maxSize = new Size(200, 200);
            WindowPosition = new Coordinates(0, 0);
            WindowSize = new Size(200, 200);
        }
        /// <summary>
        /// Gets or sets the background color of text to be written.
        /// This maps to the corresponding Console.Background property.
        /// </summary>
        public override ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the host buffer size adapted from the Console buffer 
        /// size members.
        /// </summary>
        public override Size BufferSize { get; set; }

        /// <summary>
        /// Gets or sets the cursor position. In this example this 
        /// functionality is not needed so the property throws a 
        /// NotImplementException exception.
        /// </summary>
        public override Coordinates CursorPosition
        {
            get
            {
                throw new NotImplementedException(
                     "The method or operation is not implemented.");
            }
            set
            {
                throw new NotImplementedException(
                     "The method or operation is not implemented.");
            }
        }

        /// <summary>
        /// Gets or sets the cursor size taken directly from the 
        /// Console.CursorSize property.
        /// </summary>
        public override int CursorSize { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the text to be written.
        /// This maps to the corresponding Console.ForgroundColor property.
        /// </summary>
        public override ConsoleColor ForegroundColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether a key is available. This maps to  
        /// the corresponding Console.KeyAvailable property.
        /// </summary>
        public override bool KeyAvailable
        {
            get { return _keyAvailable; }
        }

        /// <summary>
        /// Gets the maximum physical size of the window adapted from the  
        ///  Console.LargestWindowWidth and Console.LargestWindowHeight 
        ///  properties.
        /// </summary>
        public override Size MaxPhysicalWindowSize
        {
            get { return _maxSize; }
        }

        /// <summary>
        /// Gets the maximum window size adapted from the 
        /// Console.LargestWindowWidth and console.LargestWindowHeight 
        /// properties.
        /// </summary>
        public override Size MaxWindowSize
        {
            get { return _maxSize; }
        }

        /// <summary>
        /// Gets or sets the window position adapted from the Console window position 
        /// members.
        /// </summary>
        public override Coordinates WindowPosition { get; set; }

        /// <summary>
        /// Gets or sets the window size adapted from the corresponding Console 
        /// calls.
        /// </summary>
        public override Size WindowSize { get; set; }

        /// <summary>
        /// Gets or sets the title of the window mapped to the Console.Title 
        /// property.
        /// </summary>
        public override string WindowTitle { get; set; }

        /// <summary>
        /// This API resets the input buffer. In this example this 
        /// functionality is not needed so the method returns nothing.
        /// </summary>
        public override void FlushInputBuffer()
        {
        }

        /// <summary>
        /// This API returns a rectangular region of the screen buffer. In 
        /// this example this functionality is not needed so the method throws 
        /// a NotImplementException exception.
        /// </summary>
        /// <param name="rectangle">Defines the size of the rectangle.</param>
        /// <returns>Throws a NotImplementedException exception.</returns>
        public override BufferCell[,] GetBufferContents(Rectangle rectangle)
        {
            throw new NotImplementedException(
                     "The method or operation is not implemented.");
        }

        /// <summary>
        /// This API Reads a pressed, released, or pressed and released keystroke 
        /// from the keyboard device, blocking processing until a keystroke is 
        /// typed that matches the specified keystroke options. In this example 
        /// this functionality is not needed so the method throws a
        /// NotImplementException exception.
        /// </summary>
        /// <param name="options">Options, such as IncludeKeyDown,  used when 
        /// reading the keyboard.</param>
        /// <returns>Throws a NotImplementedException exception.</returns>
        public override KeyInfo ReadKey(ReadKeyOptions options)
        {
            throw new NotImplementedException(
                      "The method or operation is not implemented.");
        }

        /// <summary>
        /// This API crops a region of the screen buffer. In this example 
        /// this functionality is not needed so the method throws a
        /// NotImplementException exception.
        /// </summary>
        /// <param name="source">The region of the screen to be scrolled.</param>
        /// <param name="destination">The region of the screen to receive the 
        /// source region contents.</param>
        /// <param name="clip">The region of the screen to include in the operation.</param>
        /// <param name="fill">The character and attributes to be used to fill all cell.</param>
        public override void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill)
        {
            throw new NotImplementedException(
                      "The method or operation is not implemented.");
        }

        /// <summary>
        /// This API copies an array of buffer cells into the screen buffer 
        /// at a specified location. In this example this  functionality is 
        /// not needed si the method  throws a NotImplementedException exception.
        /// </summary>
        /// <param name="origin">The parameter is not used.</param>
        /// <param name="contents">The parameter is not used.</param>
        public override void SetBufferContents(Coordinates origin, BufferCell[,] contents)
        {
            throw new NotImplementedException(
                      "The method or operation is not implemented.");
        }

        /// <summary>
        /// This API Copies a given character, foreground color, and background 
        /// color to a region of the screen buffer. In this example this 
        /// functionality is not needed so the method throws a
        /// NotImplementException exception./// </summary>
        /// <param name="rectangle">Defines the area to be filled. </param>
        /// <param name="fill">Defines the fill character.</param>
        public override void SetBufferContents(Rectangle rectangle, BufferCell fill)
        {
            throw new NotImplementedException(
                      "The method or operation is not implemented.");
        }
    }

}
