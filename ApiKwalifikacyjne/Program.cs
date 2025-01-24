using ApiKwalifikacyjne;
using ApiKwalifikacyjne.Helpers;
using ApiKwalifikacyjne.Providers;
using ApiKwalifikacyjne.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<ILogger>(s => s.GetRequiredService<ILogger<Program>>());

builder.Services.AddDbContext<DataContext>();

builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddScoped<ISoApiService, SoApiService>();
builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

var configuration = app.Services.GetRequiredService<IConfiguration>();
var initialiseDb = configuration.GetValue("InitialiseDb", defaultValue: true);
if (initialiseDb)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        context.Database.Migrate();
        var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();
        await dataService.FetchData();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}