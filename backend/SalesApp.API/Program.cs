using Microsoft.EntityFrameworkCore;
using SalesApp.API.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("SQLConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<SalesApp.API.Business.SalesBusiness>();

builder.Services.AddCors(options => 
{
    options.AddPolicy("SalesWeb", policy => 
    {
        policy.WithOrigins("http://localhost:4200");
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "SalesApp API";
        options.Theme = ScalarTheme.Laserwave;
    });
}

app.UseHttpsRedirection();

app.UseCors("SalesWeb");

app.UseAuthorization();

app.MapControllers();

app.Run();
