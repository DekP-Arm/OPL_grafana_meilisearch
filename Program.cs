using Microsoft.EntityFrameworkCore;
using OPL_grafana_meilisearch;
using OPL_grafana_meilisearch.src.Core.Interface;
using OPL_grafana_meilisearch.src.Core.Service;
using OPL_grafana_meilisearch.src.Infrastructure.Interface;
using OPL_grafana_meilisearch.src.Infrastructure.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

// builder.Host.UseSerilog((context,configuration) => configuration
//     .ReadFrom.Configuration(context.Configuration));


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IFailedRepository,FailedRepository>();
builder.Services.AddScoped<IFailedService, FailedService>();


//add IHttpContextAccesstor
builder.Services.AddHttpContextAccessor();

//add interceptor

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();