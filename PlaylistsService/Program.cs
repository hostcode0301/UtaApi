using Microsoft.Extensions.Options;
using PlaylistsService.Data;
using PlaylistsService.Events;
using PlaylistsService.Models;
using PlaylistsService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// MongoDb Config
builder.Services.Configure<MongoDbSetting>(builder.Configuration.GetSection("MongoDbSetting"));
builder.Services.AddSingleton<MongoDbSetting>(st => st.GetRequiredService<IOptions<MongoDbSetting>>().Value);
builder.Services.AddSingleton<IMongoContext, MongoContext>();

// Auto Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Grpc client
builder.Services.AddScoped<IGrpcSongsClient, GrpcSongsClient>();

// RabbitMq Subcriber
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubcriber>();

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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
