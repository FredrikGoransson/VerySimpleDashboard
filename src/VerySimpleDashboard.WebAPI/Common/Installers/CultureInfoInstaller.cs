using System.Globalization;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using VerySimpleDashboard.WebAPI.Common.Http;

namespace VerySimpleDashboard.WebAPI.Common.Installers
{
    public class CultureInfoInstaller : IWindsorInstaller
    {
        public void Install(
            IWindsorContainer container,
            IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<ICultureInfoProvider>()
                    .ImplementedBy<HttpRequestCultureInfoProvider>()
                    .LifestylePerWebRequest(),
                Component
                    .For<CultureInfo>()
                    .UsingFactoryMethod(kernel => kernel.Resolve<ICultureInfoProvider>().GetCurrentCultureInfo())
                    .LifestylePerWebRequest()
                );
        }
    }
}