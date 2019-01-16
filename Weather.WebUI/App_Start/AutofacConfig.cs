using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Hangfire;
using Weather.Domain.Abstract;
using Weather.Domain.Concrete;
using Weather.WebUI.Jobs;
using Weather.WebUI.Services;

namespace Weather.WebUI
{
    public class AutofacConfig
    {
        public static void RegisterServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterAssemblyModules(typeof(MvcApplication).Assembly);

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<OpenWeatherMapService>().As<IExternalApiWeatherService>().InstancePerLifetimeScope();
            builder.RegisterType<EFCityRepository>().As<ICityRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EFWeatherRepository>().As<IWeatherRepository>().InstancePerLifetimeScope();

            builder.RegisterType<ReadActualWeatherForAllCitiesJob>();
            builder.RegisterType<ReadActualWeatherForOneCityJob>();

            var container = builder.Build();

            System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.UseActivator(new AutofacJobActivator(container));
        }
    }
}