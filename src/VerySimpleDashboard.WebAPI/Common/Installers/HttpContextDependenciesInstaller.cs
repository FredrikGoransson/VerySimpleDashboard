using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using VerySimpleDashboard.WebAPI.Common.Http;

namespace VerySimpleDashboard.WebAPI.Common.Installers
{
    public class HttpContextDependenciesInstaller : IWindsorInstaller
    {
        public void Install(
            IWindsorContainer container,
            IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IHtmlDecoder>()
                    .ImplementedBy<HttpContextHtmlDecoder>()
                    .LifestylePerWebRequest()
                );
        }
    }
}