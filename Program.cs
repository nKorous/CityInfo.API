using CityInfo.API;
using CityInfo.API.DbContexts;
using CityInfo.API.Service;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args); //Creates a webapp
/*** This is for the built in logger ***/
// builder.Logging.ClearProviders(); // Removes all logging providers
// builder.Logging.AddConsole(); // Manually adds the console Logger


/*** This is for using Serilog ***/
builder.Host.UseSerilog();

builder.Services.AddControllers(options => // Registers services when creating API's
{
    /*** First in the list is the default ***/
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson() // Replaces the default JSON parser and uses the NewtonsoftJson package
.AddXmlDataContractSerializerFormatters(); // Added to serialize/deserialize XML
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); // Swagger
builder.Services.AddSwaggerGen(); // Swagger
builder.Services.AddSingleton<FileExtensionContentTypeProvider>(); // Finds the file content type so you don't have to explicitly set it on each file

/*** My Services ***/

#if DEBUG // Only uses when in DEBUG MODE
builder.Services.AddTransient<IMailService, LocalMailService>(); // We can now inject this service into any controller. This is like registering a service with a provider in Angular.
#else // Only uses in Release Mode
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

builder.Services.AddSingleton<CitiesDataStore>(); // This is our DI CitiesDataStore Service/Singleton

builder.Services.AddDbContext<CityInfoContext>(
    dbContextOptions => dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));

builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>(); // This is how we inject the repository

var app = builder.Build(); // Once registered, the webApp can be built

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // Only add Swagger middleware in Dev mode
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/** Marks the position in the middleware pipeline where a routing decision is made. ***/
app.UseRouting(); 

app.UseAuthorization();

/*** Marks the position in the middleware pipeline where the selected endpoint is executed ***/
app.UseEndpoints(endpoints => 
{
    endpoints.MapControllers(); // Routes are specified on the controllers with attributes
});

// app.MapControllers();


// Middleware to have every request type out Hello, World
// app.Run(async (context) => 
// {
//     await context.Response.WriteAsync("Hello, World");
// });

app.Run();
