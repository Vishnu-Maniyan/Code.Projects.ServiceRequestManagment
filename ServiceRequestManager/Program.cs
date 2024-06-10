using Microsoft.EntityFrameworkCore;
using ServiceRequestManager.Providers;
using ServiceRequestManager.Context;
using ServiceRequestManager.Controllers;
using ServiceRequestManager.EmailService;
using ServiceRequestManager.Interfaces;
using ServiceRequestManager.Repository;
using System.Text.Json.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers()
            .AddJsonOptions(opt => { opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<ServiceRequestContext>(options => options.UseInMemoryDatabase(builder.Configuration["DATABASE_NAME"]));
        builder.Services.AddScoped<ServiceRequestContext, ServiceRequestContext>();
        builder.Services.AddTransient<IServiceRequestRepository, ServiceRequestRepository>();
        builder.Services.AddTransient<IServiceRequestProvider, ServiceRequestProvider>();
        builder.Services.AddTransient<IEmailClient, EmailClient>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        };

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}