using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;

namespace VerySimpleDashboard.WebAPI.Common.MVC
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IWindsorContainer _container;

        public WindsorControllerFactory(IWindsorContainer container)
        {
            _container = container;
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            return base.CreateController(requestContext, controllerName);
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, System.Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404,
                    string.Format("The controller for path '{0}' could not be found.",
                        requestContext.HttpContext.Request.Path));
            }
            return (IController)_container.Resolve(controllerType);
        }

        public override void ReleaseController(IController controller)
        {
            _container.Release(controller);
        }
    }
}