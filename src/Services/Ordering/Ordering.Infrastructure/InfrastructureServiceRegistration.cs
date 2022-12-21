using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Infrastructure.Mailing;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure;

public static class InfrastructureServiceRegistration
{
  public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
  {
    // SqlServer database related dependencies
    services.AddDbContext<OrderContext>(options =>
    {
      options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString"));
    });

    services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
    services.AddScoped<IOrderRepository, OrderRepository>();

    // SendGrid Email service related dependencies
    services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));
    services.AddTransient<IEmailService, EmailService>();
  }
}