using Microsoft.AspNetCore.Identity; // IdentityUser
using Microsoft.EntityFrameworkCore; // UseSqlServer, UseSqlite
using Northwind.Mvc.Data; // ApplicationDbContext
using Shared; // AddNorthwindContext extension method
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient(name: "Northwind.WebApi",
	configureClient: options =>
	{
		options.BaseAddress = new Uri("https://localhost:5002/");
		options.DefaultRequestHeaders.Accept.Add(
			new MediaTypeWithQualityHeaderValue("application/json", 1.0));
	});

builder.Services.AddHttpClient(name: "Minimal.WebApi",
	configureClient: options =>
	{
		options.BaseAddress = new Uri("https://localhost:5003/");
		options.DefaultRequestHeaders.Accept.Add(
			new MediaTypeWithQualityHeaderValue("application/json", 1.0));
	});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

string sqlServerConnection = builder.Configuration.GetConnectionString("NorthwindConnection");

builder.Services.AddNorthwindContext();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
