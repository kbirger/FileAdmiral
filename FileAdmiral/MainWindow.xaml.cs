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

namespace FileAdmiral
{
    public class UseFirstArgumentAsNameInstanceProvider : StandardInstanceProvider
    {
        protected override string GetName(System.Reflection.MethodInfo methodInfo, object[] arguments)
        {
            return (string)arguments[0];
        }

        /// <summary>
        /// Gets the constructor arguments that shall be passed with the instance request.
        /// </summary>
        /// <param name="methodInfo">The method info of the method that was called on the factory.</param><param name="arguments">The arguments that were passed to the factory.</param>
        /// <returns>
        /// The constructor arguments that shall be passed with the instance request.
        /// </returns>
        protected override IConstructorArgument[] GetConstructorArguments(MethodInfo methodInfo, object[] arguments)
        {
            var v = base.GetConstructorArguments(methodInfo, arguments).Skip(1).ToArray();
            return v;
        }
    }

    public interface IFileSystemViewModelFactoryProvider
    {
        IFileSystemViewModel Get(string type, string startPath);
    }
    public interface IFileSystemViewModelFactory
    {
        IFileSystemViewModel CreateViewModel(string startPath);

        void Register(Type type, Predicate<string> uriTestPredicate);
    }

    public class FileSystemViewModelFactory : IFileSystemViewModelFactory
    {
        private IFileSystemViewModelFactoryProvider _provider;
        public FileSystemViewModelFactory(IFileSystemViewModelFactoryProvider provider)
        {
            _provider = provider;
        }
        private List<Tuple<Predicate<string>, string>> _registry = new List<Tuple<Predicate<string>, string>>(); 
        public IFileSystemViewModel CreateViewModel(string startPath)
        {
            var typeToUse = _registry.Single(test => test.Item1(startPath)).Item2;
            return _provider.Get(typeToUse, startPath);
        }

        public void Register(Type type, Predicate<string> uriTestPredicate)
        {
            _registry.Add(new Tuple<Predicate<string>, string>(uriTestPredicate, type.FullName));
        }
    }

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
            _kernel.Bind<IFileSystemViewModelFactory>().To<FileSystemViewModelFactory>();
            _kernel.Bind<ICommandShellViewModel>().To<PowerShellViewModel>();
            _kernel.Bind<IFileSystemViewModelFactoryProvider>().ToFactory(() => new UseFirstArgumentAsNameInstanceProvider());
            _kernel.Bind<IFileSystemViewModel>().To<FolderViewModel>().Named(typeof (FolderViewModel).FullName);
            try
            {
                var fact = _kernel.Get<IFileSystemViewModelFactory>();
                fact.Register(typeof(FolderViewModel), (s) => true);
                var v = fact.CreateViewModel("C:\\");
            }
            catch (Exception ex)
            {
                
                throw;
            }
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
                //PowerShellViewModel cpvm = new PowerShellViewModel("C:\\");
                //ICommandShellViewModel cpvm = new PowerShellViewModel("C:\\");
                //CommandPromptInteropViewModel cpvm = new CommandPromptInteropViewModel("C:\\", hostPanel.Handle, hostPanel.Width, hostPanel.Height);
                DataContext = new MainViewModel();
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
