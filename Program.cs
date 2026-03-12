using GiveAID.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


// Add DbContext with SQL Server 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//For session management (Login/Logout)
builder.Services.AddSession();

var app = builder.Build();

//Serve CSS, JS, and image files from wwwroot folder
app.UseStaticFiles();
// Enable URL routing
app.UseRouting();
// Enable session (storing login state, etc.)
app.UseSession();
// Enable role-based authorization for admin/user access control
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
