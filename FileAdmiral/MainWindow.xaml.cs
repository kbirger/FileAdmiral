using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FileAdmiral.Engine.ViewModels;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Factory;
using Ninject.Extensions.Factory.Factory;
using Ninject.Parameters;
using Ninject.Extensions.Conventions;
using FileAdmiral.IoC;

namespace FileAdmiral
{   
    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IKernel _kernel = new StandardKernel();
        public MainWindow()
        {
            InitializeComponent();
            //_kernel.Bind(x => x.FromThisAssembly().sel)
            _kernel.Bind<IFileSystemViewModelFactory>().To<FileSystemViewModelFactory>().InSingletonScope();
            _kernel.Bind<IMainViewModel>().To<MainViewModel>().InSingletonScope();
            _kernel.Bind<ICommandShellViewModel>().To<PowerShellViewModel>();
            _kernel.Bind<IFileSystemViewModelProvider>().ToFactory(() => new UseFirstArgumentAsNameInstanceProvider());
            _kernel.Bind<IFileSystemViewModel>().To<FolderViewModel>().InTransientScope().Named(typeof (FolderViewModel).FullName);
            
            //_kernel.Bind<IFileSystemViewModel>().ToProvider<P>();
            //var v = _kernel.Get<IFileSystemViewModel>(x=>true, new Parameter("Path", "C:\\", false));

        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Window.ContentRendered"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            try
            {
                try
                {
                    var fact = _kernel.Get<IFileSystemViewModelFactory>();
                    fact.Register(typeof(FolderViewModel), (s) => System.Text.RegularExpressions.Regex.IsMatch(s, "(^[A-Z]:)|(file:///)"));
                    var v = fact.CreateViewModel("C:\\");
                }
                catch (Exception ex)
                {

                    throw;
                }
                //PowerShellViewModel cpvm = new PowerShellViewModel("C:\\");
                //ICommandShellViewModel cpvm = new PowerShellViewModel("C:\\");
                //CommandPromptInteropViewModel cpvm = new CommandPromptInteropViewModel("C:\\", hostPanel.Handle, hostPanel.Width, hostPanel.Height);
                var viewModel = _kernel.Get<IMainViewModel>();
                viewModel.Initialize("C:\\", "C:\\Program Files");
                DataContext = viewModel;


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void CommandInput_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ((MainViewModel)DataContext).CommandShell.SendCommand(CommandInput.Text);
                CommandInput.Text = "";
            }
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = ((TextBox)sender);
            textBox.CaretIndex = textBox.Text.Length;
            textBox.ScrollToEnd();
            //CommandInput.Padding = new Thickness(PromptDisplay.ActualWidth + 4, CommandInput.Padding.Top, CommandInput.Padding.Right, CommandInput.Padding.Bottom);
        }
    }
}
