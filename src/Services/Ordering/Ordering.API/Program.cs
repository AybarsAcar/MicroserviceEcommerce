using EventBus.Messages.Common;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.API.Middlewares;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configure injectable dependencies
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// RabbitMQ Configuration as a Consumer
builder.Services.AddMassTransit(config =>
{
  config.AddConsumer<BasketCheckoutConsumer>();

  config.UsingRabbitMq((ctx, cfg) =>
  {
    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

    cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueueName,
      configurator => { configurator.ConfigureConsumer<BasketCheckoutConsumer>(ctx); });
  });
});

builder.Services.AddScoped<BasketCheckoutConsumer>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

app.MigrateDatabase<OrderContext>((context, services) =>
{
  var logger = services.GetService<ILogger<OrderContextSeed>>();
  OrderContextSeed.SeedAsync(context, logger).Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

// app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();