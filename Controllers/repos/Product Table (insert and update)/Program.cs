using Microsoft.EntityFrameworkCore;
using Project2.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Mydbcontext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("dbcon")));

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
app.MapControllerRoute("default","{controller=Product}/{action=Insert}");

app.Run();
