using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Weather.Domain.Abstract;
using Weather.Domain.Concrete;
using Weather.WebUI.Services;

namespace Weather.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IExternalApiWeatherService>().To<OpenWeatherMapService>();
            kernel.Bind<ICityRepository>().To<EFCityRepository>();
            kernel.Bind<IWeatherRepository>().To<EFWeatherRepository>();
        }
    }
}