using EventApp.Core.Services;
using EventApp.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventTypeService, EventTypeService>();
builder.Services.AddScoped<ITicketTypeService, TicketTypeService>();

// Configure the database context and connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EventAppDbContext>(options => options.UseSqlServer(connectionString, x => x.MigrationsAssembly("EventApp.DAL")).EnableDetailedErrors(true));

builder.Services.AddControllers();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.WithOrigins("https://localhost:7087")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add logging services
builder.Services.AddLogging(config =>
{
    config.AddDebug();
    config.AddConsole();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    var filePath = Path.Combine(System.AppContext.BaseDirectory, "EventApp.API.xml");
    c.IncludeXmlComments(filePath);
});

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("MyPolicy"); // Make sure this is after UseRouting

app.UseAuthorization();

app.MapControllers();

app.Run();
