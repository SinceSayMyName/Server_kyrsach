using Tracker.TelegramBot.Middleware;
using TripPlanner.Controllers;
using TripPlanner.DBTripPlanner;
using TripPlanner.DBTripPlanner.CSVDataLoader;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<DBApplicationContext>();
builder.Services.AddTransient<Converter>();
builder.Services.AddTransient<DBCityLoader>();
builder.Services.AddTransient<DBHelper>();

var app = builder.Build();

using var serviceProvider = builder.Services.BuildServiceProvider();
DBCityLoader? loader = serviceProvider.GetService<DBCityLoader>();
DBHelper? helper = serviceProvider.GetService<DBHelper>();

loader.LoadIfEmpty();
helper.GenerateTestDataIfEmpty();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<OptionsMiddleware>();
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();