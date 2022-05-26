using Microsoft.EntityFrameworkCore;
using SongsService.Data;
using SongsService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// PostgreSQL context
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// gRPC clients
builder.Services.AddScoped<IGrpcArtistsClient, GrpcArtistsClient>();

// gRPC server
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

// RabitMq publisher
builder.Services.AddSingleton<IMessageBusPublisher, MessageBusPublisher>();

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

app.MapGrpcService<GrpcSongsServer>();

app.Run();
