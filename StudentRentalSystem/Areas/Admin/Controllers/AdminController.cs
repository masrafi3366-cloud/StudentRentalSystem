using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;


namespace StudentRentalSystem.Areas.Admin.Controllers
{


    [Area("Admin")]
    public class AdminController : Controller
    {


        private readonly ApplicationDbContext _context;


        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }




        public IActionResult Login()
        {
            return View();
        }






        [HttpPost]
        public IActionResult Login(
            string email,
            string password
        )
        {


            var admin =
            _context.Admins
            .FirstOrDefault(
                x =>
                x.Email == email
                &&
                x.Password == password
            );



            if (admin == null)
            {

                ViewBag.Error =
                "Invalid admin login";


                return View();

            }



            HttpContext.Session.SetString(
                "Admin",
                admin.Email
            );



            return RedirectToAction(
                "Index",
                "Dashboard"
            );


        }





        public IActionResult Logout()
        {

            HttpContext.Session.Remove(
                "Admin"
            );


            return RedirectToAction(
                "Login"
            );

        }


    }

}