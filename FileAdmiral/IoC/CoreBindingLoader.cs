using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileAdmiral.Engine.ViewModels;
using FileAdmiral.Engine.Views;
using FileAdmiral.Views;
using Ninject.Extensions.Factory;
using Ninject.Modules;

namespace FileAdmiral.IoC
{
    internal class CoreBindingLoader : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IMainViewModel>().To<MainViewModel>().InSingletonScope();
            Bind<ICommandShellViewModel>().To<PowerShellViewModel>();

            Bind<IFileSystemViewModelFactory>().To<FileSystemViewModelFactory>().InSingletonScope();
            Bind<IFileSystemViewModelProvider>().ToFactory(() => new UseFirstArgumentAsNameInstanceProvider()).InSingletonScope();
            
            Bind<IFileSystemViewModel>().To<FolderViewModel>().InTransientScope().Named(typeof(FolderViewModel).FullName);
            
            Bind<IViewFactory>().To<ViewFactory>();
            Bind<IFileSystemView>().To<SimpleFileSystemView>();
            Bind<IMainView>().To<MainView>();
            Bind<ICommandShellView>().To<CommandShellView>();
        }
    }
}
