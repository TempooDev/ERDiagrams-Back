using System;
using System.Reflection;
using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Settings;
using ERDiagrams.Back;
using ERDiagrams.Back.Models;
using ERDiagrams.Back.Repositories;
using ERDiagrams.Back.Repositories.Interfaces;
using ERDiagrams.Back.Services;
using ERDiagrams.Back.Services.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

[assembly: FunctionsStartup(typeof(Startup))]

namespace ERDiagrams.Back
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly(), opts =>
            {
                opts.AddCodeParameter = true;
                opts.Documents = new[]
                {
                    new SwaggerDocument
                    {
                        Name = "v1",
                        Title = "Swagger document", Description = "Swagger",
                        Version = "v1"
                    }
                };
                opts.ConfigureSwaggerGen = x =>
                {
                    x.CustomOperationIds(apiDesc =>
                    {
                        return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)
                            ? methodInfo.Name
                            : default(Guid).ToString();
                    });
                };
            });
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
            
            services.AddDbContext<CosmosContext>();

            #region Repositories
            services.AddScoped<IRepository<Diagram>, DiagramRepository>();
            

            #endregion

            #region Services

            
            services.AddScoped<IDiagramService, DiagramService>();

            #endregion
            
            return services;
        }
    }
}