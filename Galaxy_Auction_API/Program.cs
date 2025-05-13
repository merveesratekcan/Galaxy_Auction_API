using Galaxy_Auction_API.Extensions;
using Galaxy_Auction_Business.Abstraction;
using Galaxy_Auction_Business.Concrete;
using Galaxy_Auction_Business.Mapper;
using Galaxy_Auction_Core.Models;
using Galaxy_Auction_Data_Access.Context;
using Galaxy_Auction_Data_Access.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddApplicationLayer(builder.Configuration);
builder.Services.AddInfractructureLayer(builder.Configuration);
builder.Services.AddSwaggerCollection(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Galaxy Auction API", Version = "v1" });
});

//Authomapper Configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
//    });
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Galaxy Auction API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors(
    x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials()
    );
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
