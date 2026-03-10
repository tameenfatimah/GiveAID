using image_uploading.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Mydbcontext>(opt=>opt.UseSqlServer(builder.Configuration.GetConnectionString("con")));
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.MapControllerRoute("default","{controller=Home}/{action=Index}");

//app.MapGet("/", () => "Hello World!");

app.Run();
