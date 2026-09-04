using Microsoft.EntityFrameworkCore;
using StudentRentalSystem.Data;


var builder = WebApplication.CreateBuilder(args);




// =========================
// MVC SERVICE
// =========================

builder.Services.AddControllersWithViews();




// =========================
// DATABASE CONNECTION
// =========================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"
        )
    )
);




// =========================
// SESSION
// =========================

builder.Services.AddSession();





var app = builder.Build();





// =========================
// DATABASE INITIALIZER
// =========================

using (var scope = app.Services.CreateScope())
{

    var context =
    scope.ServiceProvider
    .GetRequiredService<ApplicationDbContext>();


    DbInitializer.Initialize(context);

}






// =========================
// ERROR HANDLING
// =========================

if (!app.Environment.IsDevelopment())
{

    app.UseExceptionHandler(
        "/Home/Error"
    );


    app.UseHsts();

}






app.UseHttpsRedirection();



app.UseStaticFiles();



app.UseRouting();





// =========================
// SESSION
// =========================

app.UseSession();





app.UseAuthorization();







// =========================
// ADMIN AREA ROUTING
// =========================

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);








// =========================
// DEFAULT ROUTING
// =========================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);






app.Run();