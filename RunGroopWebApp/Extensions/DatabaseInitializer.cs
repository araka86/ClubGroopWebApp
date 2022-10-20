using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ClubGroopWebApp.Extensions
{
    public static class DatabaseInitializer
    {

        public static async Task InitializeDatabase<T>(this IApplicationBuilder app) where T : DbContext
        {
            using var scope = app.ApplicationServices.CreateScope();
            await using var context = ActivatorUtilities.CreateInstance<T>(scope.ServiceProvider);

            if ((await context.Database.GetPendingMigrationsAsync().ConfigureAwait(false)).Any())
            {
                Log.Information("Migration for context {context} available", context.GetType());
                await context.Database.MigrateAsync().ConfigureAwait(false);
            }
            else
            {
                Log.Information("No migration for context {context} available", context.GetType());
            }

            await context.DisposeAsync().ConfigureAwait(false);


        }

    }
}
