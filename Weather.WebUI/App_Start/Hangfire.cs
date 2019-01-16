using Microsoft.Owin;
using Owin;
using Hangfire;
using Weather.WebUI;
using Weather.WebUI.Jobs;

[assembly: OwinStartup(typeof(Startup))]
namespace Weather.WebUI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("EFDbContext");
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            RecurringJob.AddOrUpdate<ReadActualWeatherForAllCitiesJob>(x => x.Run(), Cron.Daily);
        }
    }
}