using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using VerySimpleDashboard.Importer;
using VerySimpleDashboard.WebAPI.Common.MVC;

namespace VerySimpleDashboard.WebAPI.Common.Installers
{
    public class AspNetDataPathBuilderInstaller : IWindsorInstaller
    {
        public void Install(
            IWindsorContainer container,
            IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IFilePathBuilder>()
                    .ImplementedBy<AspNetDataPathBuilder>()
                    .LifestylePerWebRequest()
                );
        }
    }
}