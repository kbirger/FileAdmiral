using System.Linq;
using FileAdmiral.Engine.ViewModels;
using FileAdmiral.Engine.Views;
using FileAdmiral.IoC;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;

namespace FileAdmiral
{
    public interface IViewFactory
    {
        TView CreateView<TView>() where TView : IView;
        TView CreateView<TView>(params ConstructorParameter[] args) where TView : IView;
    }

    public class ViewFactory : IViewFactory
    {
        private readonly IResolutionRoot _resolutionRoot;

        public ViewFactory(IResolutionRoot resolutionRoot)
        {
            _resolutionRoot = resolutionRoot;
        }

        public TView CreateView<TView>() where TView : IView
        {
            return _resolutionRoot.Get<TView>();
        }

        public TView CreateView<TView>(params ConstructorParameter[] args) where TView : IView
        {
            // convert our custom parameters to NInject ctor arguments
            var parameters = args
                .Select(arg => (IParameter)new ConstructorArgument(arg.Key, arg.Value, true))
                .ToArray();

            var view = _resolutionRoot.Get<TView>(parameters);

            return view;
        }
    }
}
