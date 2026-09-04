using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;



namespace StudentRentalSystem.Areas.Admin.Controllers
{


    [Area("Admin")]
    public class AdminController : Controller
    {


        private readonly ApplicationDbContext _context;



        public AdminController(
            ApplicationDbContext context
        )
        {

            _context = context;

        }








        // =========================
        // ADMIN LOGIN GET
        // =========================


        [HttpGet]
        [Route("Admin/Login")]
        public IActionResult Login()
        {

            return View();

        }









        // =========================
        // ADMIN LOGIN POST
        // =========================


        [HttpPost]
        [Route("Admin/Login")]
        [ValidateAntiForgeryToken]
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
                "Invalid admin email or password";


                return View();

            }







            HttpContext.Session.SetString(
                "Admin",
                admin.Email
            );







            return RedirectToAction(
                "Index",
                "Dashboard",
                new
                {
                    area = "Admin"
                }
            );


        }









        // =========================
        // ADMIN LOGOUT
        // =========================

        [HttpGet]
        public IActionResult Logout()
        {

            HttpContext.Session.Remove("Admin");


            return RedirectToAction(
                "Login",
                "Admin",
                new
                {
                    area = "Admin"
                }
            );

        }



    }

}