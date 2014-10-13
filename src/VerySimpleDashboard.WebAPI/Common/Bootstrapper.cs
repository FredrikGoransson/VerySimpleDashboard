using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace VerySimpleDashboard.WebAPI.Common
{
    public static class Bootstrapper
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IWindsorContainer ConfigureContainer<TStartController>()
        {
            Log.Debug("Configuring Windsor Container");
            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
            container.Kernel.Resolver.AddSubResolver(new ListResolver(container.Kernel));
            container.Register(Component.For<IWindsorContainer>().Instance(container));
            container.Install(FromAssembly.InThisApplication());
            var controllerFactory = container.Resolve<IControllerFactory>();
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            return container;
        }

    }
}