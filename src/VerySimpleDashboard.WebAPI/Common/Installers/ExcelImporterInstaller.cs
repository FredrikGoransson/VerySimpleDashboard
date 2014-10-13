using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using VerySimpleDashboard.Importer;

namespace VerySimpleDashboard.WebAPI.Common.Installers
{
    public class ExcelImporterInstaller : IWindsorInstaller
    {
        public void Install(
            IWindsorContainer container,
            IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IExcelReaderProxy>()
                    .ImplementedBy<ExcelReaderProxy>()
                    .LifestylePerWebRequest(), 
                Component
                    .For<IExcelImporter>()
                    .ImplementedBy<ExcelImporter>()
                    .LifestylePerWebRequest());
        }
    }
}