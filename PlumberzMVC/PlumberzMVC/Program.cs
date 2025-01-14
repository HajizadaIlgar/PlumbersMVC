using Microsoft.EntityFrameworkCore;
using PlumberzMVC.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PlumbersDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("MYSqlCode"));
});
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
