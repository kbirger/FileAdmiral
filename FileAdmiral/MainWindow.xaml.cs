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
using FileAdmiral.Engine.Views;
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
        private IKernel _kernel = new StandardKernel(new CoreBindingLoader());
        public MainWindow()
        {
            InitializeComponent();

            // todo: move this into separate class
            var fact = _kernel.Get<IFileSystemViewModelFactory>();
            fact.Register(typeof(FolderViewModel), (s) => System.Text.RegularExpressions.Regex.IsMatch(s, "(^[A-Z]:)|(file:///)"));
            //_kernel.Bind(x => x.FromThisAssembly().sel)
            

            //_kernel.Bind<IFileSystemViewModel>().ToProvider<P>();
            //var v = _kernel.Get<IFileSystemViewModel>(x=>true, new Parameter("Path", "C:\\", false));
            var viewModel = _kernel.Get<IMainViewModel>();
            viewModel.Initialize("C:\\", "C:\\Program Files");
            DataContext = viewModel;
            Content = _kernel.Get<IViewFactory>().CreateView<IMainView>();
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
                    
                    //var v = fact.CreateViewModel("C:\\");
                }
                catch (Exception ex)
                {

                    throw;
                }
                //PowerShellViewModel cpvm = new PowerShellViewModel("C:\\");
                //ICommandShellViewModel cpvm = new PowerShellViewModel("C:\\");
                //CommandPromptInteropViewModel cpvm = new CommandPromptInteropViewModel("C:\\", hostPanel.Handle, hostPanel.Width, hostPanel.Height);
                


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        
    }
}
