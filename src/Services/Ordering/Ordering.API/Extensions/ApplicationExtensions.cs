using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Ordering.API.Extensions;

public static class ApplicationExtensions
{
  /// <summary>
  /// Runs the migration on a DbContext
  /// </summary>
  /// <param name="app"></param>
  /// <param name="seeder">Seeder action</param>
  /// <param name="retry"></param>
  /// <typeparam name="TContext"></typeparam>
  /// <returns></returns>
  public static WebApplication MigrateDatabase<TContext>(
    this WebApplication app,
    Action<TContext, IServiceProvider> seeder,
    int? retry = 0
  ) where TContext : DbContext
  {
    var retryForAvailability = retry.Value;

    using var scope = app.Services.CreateScope();

    var services = scope.ServiceProvider;

    var logger = services.GetService<ILogger<TContext>>();

    var context = services.GetService<TContext>();

    try
    {
      logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext));

      InvokeSeeder(seeder, context, services);

      logger.LogInformation("Successfully migrated database associated with context {DbContextName}", typeof(TContext));
    }
    catch (PostgresException e)
    {
      logger.LogError(e, "Error migrating the database with context {DbContextName}", typeof(TContext));
      if (retryForAvailability < 50)
      {
        retryForAvailability++;
        Thread.Sleep(2000);
        MigrateDatabase(app, seeder, retryForAvailability);
      }
    }

    return app;
  }

  private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
    IServiceProvider services) where TContext : DbContext
  {
    context.Database.Migrate();
    seeder(context, services);
  }
}