using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAdmiral.Engine.ViewModels
{
    public interface IFileSystemViewModelProvider
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
        private IFileSystemViewModelProvider _provider;
        public FileSystemViewModelFactory(IFileSystemViewModelProvider provider)
        {
            _provider = provider;
        }
        private List<Tuple<Predicate<string>, string>> _registry = new List<Tuple<Predicate<string>, string>>();
        public IFileSystemViewModel CreateViewModel(string startPath)
        {
            var typeToUse = _registry.Single(test => test.Item1(startPath)).Item2;
            var viewModel = _provider.Get(typeToUse, startPath);
            viewModel.Initialize(startPath);
            return viewModel;
        }

        public void Register(Type type, Predicate<string> uriTestPredicate)
        {
            _registry.Add(new Tuple<Predicate<string>, string>(uriTestPredicate, type.FullName));
        }
    }
}
