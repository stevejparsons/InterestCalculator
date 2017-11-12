using BusinessLayer;
using BusinessLayer.Contract;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionServices
{
    public class InjectionInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IInterestCalculator>().ImplementedBy<InterestCalculator>().LifestyleTransient());
            container.Register(Component.For<IInterestFactory>().ImplementedBy<InterestFactory>().LifestyleTransient());
        }
    }
}
