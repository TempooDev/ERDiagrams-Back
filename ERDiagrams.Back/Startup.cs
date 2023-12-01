using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using ERDiagrams.Back;
using ERDiagrams.Back.Models;
using ERDiagrams.Back.Repositories;
using ERDiagrams.Back.Repositories.Interfaces;
using ERDiagrams.Back.Services;
using ERDiagrams.Back.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly : FunctionsStartup(typeof(Startup))]

namespace ERDiagrams.Back;
public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        ConfigureServices(builder.Services).BuildServiceProvider(true);
    }

    private IServiceCollection ConfigureServices(IServiceCollection services)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("local.settings.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        services.AddSingleton(new FunctionConfiguration(config));
        services.AddScoped<IRepository<Book>, BookRepository>();
        services.AddScoped<IBookService, BookService>();
        
        return services;
    }
}