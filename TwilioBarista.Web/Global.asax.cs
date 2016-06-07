using System;
using System.Configuration;
using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Mvc;
using Twilio;
using TwilioBarista.Web.DAL;
using TwilioBarista.Web.Repository;
using System.Data.Entity.Migrations;

namespace TwilioBarista.Web
{
    public class MvcApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Database.SetInitializer(new TwilioBaristaInitializer());
            ViewEngines.Engines.Clear();
            var razorViewEngine = new RazorViewEngine() { FileExtensions = new string[] { "cshtml" } };
            ViewEngines.Engines.Add(new RazorViewEngine());

            if (bool.Parse(ConfigurationManager.AppSettings["MigrateDatabaseToLatestVersion"]))
            {
                var configuration = new Migrations.Configuration();
                var migrator = new DbMigrator(configuration);
                migrator.Update();
            }
        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);

            // Install our Ninject-based IDependencyResolver into the Web API config
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

            return kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<TwilioBaristaContext>()
                .To<TwilioBaristaContext>()
                .Named("TwilioBaristaContext");

            kernel.Bind<IDrinkRepository>()
                .To<DrinkRepository>()
                .WithConstructorArgument("_db", ctx => ctx.Kernel.Get<TwilioBaristaContext>("TwilioBaristaContext"));

            kernel.Bind<IDrinkTypesRepository>()
                .To<DrinkTypesRepository>()
                .Named("DrinkTypesRepository")
                .WithConstructorArgument("_db", ctx => ctx.Kernel.Get<TwilioBaristaContext>("TwilioBaristaContext"));

            kernel.Bind<IOrderRepository>()
                .To<OrderRepository>()
                .Named("OrderRepository")
                .WithConstructorArgument("_db", ctx => ctx.Kernel.Get<TwilioBaristaContext>("TwilioBaristaContext"));

            kernel.Bind<TwilioRestClient>()
                .To<TwilioRestClient>()
                .Named("TwilioRestClient")
                .WithConstructorArgument("AccountSid", ConfigurationManager.AppSettings["TwilioSid"])
                .WithConstructorArgument("AuthToken", ConfigurationManager.AppSettings["TwilioToken"]);


            //kernel.Bind<IRealTime<Pusher>>()
            //    .To<PusherImpl>()
            //    .Named("RealTime")
            //    .WithConstructorArgument("id", ConfigurationManager.AppSettings["PusherAppId"])
            //    .WithConstructorArgument("key", ConfigurationManager.AppSettings["PusherKey"])
            //    .WithConstructorArgument("secret", ConfigurationManager.AppSettings["PusherSecret"]);

        }
    }
}
