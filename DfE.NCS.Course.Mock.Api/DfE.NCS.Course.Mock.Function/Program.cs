using Azure.Core;
using Azure.Core.Serialization;
using Azure.Identity;
using DfE.NCS.Course.Mock.Function.Configuration;
using DfE.NCS.Course.Mock.Function.Constants;
using DfE.NCS.Course.Mock.Function.Database;
using DfE.NCS.Course.Mock.Function.Storage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights()
    .AddSingleton<IPaginationSettings, PaginationSettings>()
    .AddSingleton<ISqlDBConnectionFactory, SqlConnectionFactory>()  
    .AddSingleton<ICourseStore, CourseStore>()
    .AddSingleton<ITLevelStore, TLevelStore>();

// Inject TokenCredential (DefaultAzureCredential)
builder.Services.AddSingleton<TokenCredential>(
    sp =>
    {
        var env = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");

        if (string.Equals(env, "Development", StringComparison.OrdinalIgnoreCase))
        {
            // Local development
            return new AzureCliCredential();
        }

        // Running in Azure
        return new DefaultAzureCredential();
    });

builder.Services.Configure<DatabaseSettings>(
               builder.Configuration.GetSection(ConfigConstants.DatabaseSection));
// Add memory cache services
builder.Services.AddMemoryCache();

builder.Services.Configure<WorkerOptions>(options =>
{
    options.Serializer = new JsonObjectSerializer(new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });
});

builder.Build().Run();
