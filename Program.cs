using Microsoft.EntityFrameworkCore;
using testAPI;
using testAPI.src.Core.Interface;
using testAPI.src.Core.Service;
using testAPI.src.Infrastructure.Interface;
using testAPI.src.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();




builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IFailedRepository, FailedRepository>();
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