using Microsoft.EntityFrameworkCore;
using StudentRentalSystem.Data;

var builder = WebApplication.CreateBuilder(args);


// Add MVC Services
builder.Services.AddControllersWithViews();


// Add Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


var app = builder.Build();


// Configure HTTP Request Pipeline

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseRouting();


app.UseAuthorization();


// Default Route

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


app.Run();