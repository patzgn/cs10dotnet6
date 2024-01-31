using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc.Formatters;
using Northwind.WebApi.Repositories;
using Shared;
using Swashbuckle.AspNetCore.SwaggerUI;
using static System.Console;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://localhost:5002/");

// Add services to the container.

builder.Services.AddCors();

builder.Services.AddNorthwindContext();

builder.Services.AddControllers(options =>
{
	WriteLine("Default output formatters:");
	foreach (IOutputFormatter formatter in options.OutputFormatters)
	{
		OutputFormatter? mediaFormatter = formatter as OutputFormatter;
		if (mediaFormatter == null)
		{
			WriteLine($"  {formatter.GetType().Name}");
		}
		else
		{
			WriteLine("  {0}, Media types {1}",
				arg0: mediaFormatter.GetType().Name,
				arg1: string.Join(", ", mediaFormatter.SupportedMediaTypes));
		}
	}
})
	.AddXmlDataContractSerializerFormatters()
	.AddXmlSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new() { Title = "Northwind Service API", Version = "v1" });
});

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddHttpLogging(options =>
{
	options.LoggingFields = HttpLoggingFields.All;
	options.RequestBodyLogLimit = 4096; // default is 32k
	options.ResponseBodyLogLimit = 4096; // default is 32k
});

builder.Services.AddHealthChecks()
	.AddDbContextCheck<NorthwindContext>();

var app = builder.Build();

app.UseCors(configurePolicy: options =>
{
	options.WithMethods("GET", "POST", "PUT", "DELETE");
	options.WithOrigins(
		"https://localhost:5001"
	);
});

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json",
			"Northwind Service API Version 1");

		c.SupportedSubmitMethods(new[] {
			SubmitMethod.Get, SubmitMethod.Post,
			SubmitMethod.Put, SubmitMethod.Delete });
	});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHealthChecks(path: "/howdoyoufeel");

app.UseMiddleware<SecurityHeaders>();

app.MapControllers();

app.Run();
