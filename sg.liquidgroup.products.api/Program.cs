using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Sg.LiquidGroup.Products.Api.Mapper;
using Sg.LiquidGroup.Products.Api.Services;
using Sg.LiquidGroup.Products.Dataaccess;
using FluentValidation;
using FluentValidation.AspNetCore;
using Sg.LiquidGroup.Products.Domain.Entity;
using Sg.LiquidGroup.Products.Api.Validator;

public class Program
{

    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers().AddFluentValidation();
        builder.Services.AddApiVersioning(x =>
        {
            x.DefaultApiVersion = new ApiVersion(1, 0);
            x.AssumeDefaultVersionWhenUnspecified = true;
            x.ReportApiVersions = true;
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        RegisterDomainServices(builder.Services);
        var app = builder.Build();
        RunDBMigration(app);
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }

    private static IConfiguration LoadConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, false);
        return builder.Build();
    }
    private static void RegisterDomainServices(IServiceCollection serviceDescriptors)
    {
        var config = LoadConfiguration();
        var mongoDbSettings = new MongoDbSettings();
        mongoDbSettings.ConnectionString =config.GetSection("ConnectionString").Value;
        mongoDbSettings.DatabaseName = config.GetSection("DatabaseName").Value;

        serviceDescriptors.AddSingleton<IMongoDatabase>(sp =>
        {

            var client = new MongoClient(mongoDbSettings.ConnectionString);
            return client.GetDatabase(mongoDbSettings.DatabaseName);
        });

        serviceDescriptors.AddSingleton<IMongoDbSettings>(mongoDbSettings);
        serviceDescriptors.AddScoped<IProductService, ProductService>();
        serviceDescriptors.AddScoped<ICartService, CartService>();
        serviceDescriptors.AddSingleton<IECommerceRepository, ECommerceRepository>();//TODO

        #region Mapper
        serviceDescriptors.AddAutoMapper(typeof(AutoMapperProfile));
        #endregion


        #region Fluent Validator
        serviceDescriptors.AddTransient<IValidator<OrderRequest>, OrderRequestValidator>();
        #endregion


    }

    private static void RunDBMigration(WebApplication app)
    {
        var ecommerceRepo = app.Services.GetService<IECommerceRepository>();
        Task.Run(async () => await ecommerceRepo.AddMasterData());
    }

}
