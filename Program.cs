using Microsoft.EntityFrameworkCore;
using OPL_grafana_meilisearch;
using OPL_grafana_meilisearch.src.Core.Interface;
using OPL_grafana_meilisearch.src.Core.Service;
using OPL_grafana_meilisearch.src.Infrastructure.Interface;
using OPL_grafana_meilisearch.src.Infrastructure.Repositories;
using Serilog;
using Infisical.Sdk;
using Meilisearch;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OPL_grafana_meilisearch
{
    public class Program
    {
        public static ConfigurationManager? Configuration { get; private set; }
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Configuration = builder.Configuration;


            // Enable CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader();
                });
            });

            // Add controllers
            builder.Services.AddControllers();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IFailedRepository, FailedRepository>();
            builder.Services.AddScoped<IFailedService, FailedService>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "OPL_grafana_meilisearch", Version = "v1" });
            });

            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            // OpenTelemetry setup
            Action<ResourceBuilder> appResourceBuilder =
                resource => resource.AddTelemetrySdk()
                                    .AddService(Configuration.GetValue<string>("Otlp:ServiceName"));

            builder.Services.AddOpenTelemetry()
                .ConfigureResource(appResourceBuilder)
                .WithTracing(tracingBuilder => tracingBuilder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSource("APITracing")
                    .AddOtlpExporter(options => options.Endpoint = new Uri(Configuration.GetValue<string>("Otlp:Endpoint")))
                )
                .WithMetrics(metricsBuilder => metricsBuilder
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(options => options.Endpoint = new Uri(Configuration.GetValue<string>("Otlp:Endpoint"))));

            // Configure Entity Framework Core with PostgreSQL
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure Serilog
            builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .WriteTo.OpenTelemetry(options =>
                {
                    options.Endpoint = $"{Configuration.GetValue<string>("Otlp:Endpoint")}/v1/logs";
                    options.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.Grpc;
                    options.ResourceAttributes = new Dictionary<string, object>
                    {
                        { "service.name", Configuration.GetValue<string>("Otlp:ServiceName") }
                    };
                }).WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day));
            
            builder.Services.AddHealthChecks();
            builder.Services.AddHttpClient();
            
            
            builder.Services.AddSingleton<TokenService>();
           
            var app = builder.Build();

            // Configure the HTTP request pipeline
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OPL_grafana_meilisearch v1"));

            app.UseCors();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.UseSerilogRequestLogging();

            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                AllowCachingResponses = false,
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                }
            });


            
            app.Run();
        }
    }
}
