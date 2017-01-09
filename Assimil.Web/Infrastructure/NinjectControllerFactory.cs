using System.Web.Mvc;
using Ninject;
using Assimil.Core;

namespace Assimil.Web
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, System.Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            // Add bindings here
            ninjectKernel.Bind<IAssimil>().To<AssimilProvider>();
            ninjectKernel.Bind<ILesson>().To<LessonProvider>();
        }
    }

    
}