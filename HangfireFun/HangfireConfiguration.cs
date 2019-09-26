using System;
using Hangfire;
using Hangfire.SqlServer;
using Owin;

namespace HangfireFun
{
    public static class HangfireConfiguration  
    {
        public static void HangfireInit(string database, IAppBuilder app)
        {

            GlobalConfiguration.Configuration
                .UseSqlServerStorage(database,
                 new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(1)});

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}
