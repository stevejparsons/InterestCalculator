using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Routing;

namespace WebService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private readonly IWindsorContainer container;

        public WebApiApplication()
        {
            var injectionInstaller = new InjectionServices.InjectionInstaller();

            this.container = new WindsorContainer().Install(injectionInstaller);

            this.container.Register(Classes.FromThisAssembly().BasedOn<IHttpController>().LifestylePerWebRequest());
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorCompositionRoot(this.container));

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        public override void Dispose()
        {
            this.container.Dispose();
            base.Dispose();
        }
    }
}
