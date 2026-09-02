using Microsoft.EntityFrameworkCore;
using StudentRentalSystem.Data;


var builder = WebApplication.CreateBuilder(args);



// MVC Service

builder.Services.AddControllersWithViews();




// Database Connection

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"
        )
    )
);




// Session

builder.Services.AddSession();



var app = builder.Build();




// Error Handling

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



app.UseSession();



app.UseAuthorization();





// Area Route

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);






// Default Route

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);




app.Run();