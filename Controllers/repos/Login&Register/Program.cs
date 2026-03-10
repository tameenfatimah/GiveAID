using Login_Register.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Mydbcontext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession();
var app = builder.Build();


app.MapControllerRoute("default","{controller=Home}/{action=Register}");
//app.MapGet("/", () => "Hello World!");

app.UseSession();
app.UseRouting();
app.Run();
