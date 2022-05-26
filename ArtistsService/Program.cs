using ArtistsService.Data;
using ArtistsService.Models;
using ArtistsService.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// MongoDb config
builder.Services.Configure<MongoDbSetting>(builder.Configuration.GetSection("MongoDbSetting"));
builder.Services.AddSingleton<MongoDbSetting>(st => st.GetRequiredService<IOptions<MongoDbSetting>>().Value);
builder.Services.AddSingleton<IMongoContext, MongoContext>();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Grpc Services
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGrpcReflectionService();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<GrpcArtistsServer>();

app.Run();
