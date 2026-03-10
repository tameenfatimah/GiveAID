using furni.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//Database Connection   
builder.Services.AddDbContext<ApplicationDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Identity 
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
{ options.SignIn.RequireConfirmedAccount = false;}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("default","{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
