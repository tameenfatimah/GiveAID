using luxur.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("con")));
var app = builder.Build();

app.MapControllerRoute("default","{controller=Home}/{action=Index}");

app.Run();
