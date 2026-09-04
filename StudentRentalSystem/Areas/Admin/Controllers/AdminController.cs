using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models.ViewModels;



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
        // ADMIN PAYMENTS
        // =========================


        [Route("Admin/Payments")]
        public IActionResult Payments()
        {


            if (
                HttpContext.Session.GetString(
                    "Admin"
                )
                == null
            )
            {

                return RedirectToAction(
                    "Login",
                    "Admin",
                    new
                    {
                        area = "Admin"
                    }
                );

            }








            var payments =
            _context.Payments
            .Select(
                p =>
                new AdminPaymentViewModel
                {


                    PaymentId =
                    p.PaymentId,



                    Amount =
                    p.Amount,



                    PaymentMethod =
                    p.PaymentMethod,



                    TransactionId =
                    p.TransactionId,



                    PaymentStatus =
                    p.PaymentStatus,



                    PaymentDate =
                    p.PaymentDate,







                    StudentName =
                    _context.Students
                    .Where(
                        s =>
                        s.StudentId ==
                        _context.Rentals
                        .Where(
                            r =>
                            r.RentalId ==
                            p.RentalId
                        )
                        .Select(
                            r =>
                            r.StudentId
                        )
                        .FirstOrDefault()
                    )
                    .Select(
                        s =>
                        s.FullName
                    )
                    .FirstOrDefault()
                    ??
                    "Unknown",








                    ItemName =
                    _context.Items
                    .Where(
                        i =>
                        i.ItemId ==
                        _context.Rentals
                        .Where(
                            r =>
                            r.RentalId ==
                            p.RentalId
                        )
                        .Select(
                            r =>
                            r.ItemId
                        )
                        .FirstOrDefault()
                    )
                    .Select(
                        i =>
                        i.ItemName
                    )
                    .FirstOrDefault()
                    ??
                    "Unknown"



                }
            )
            .OrderByDescending(
                x =>
                x.PaymentDate
            )
            .ToList();








            return View(
                payments
            );


        }









        // =========================
        // ADMIN LOGOUT
        // =========================


        [HttpGet]
        public IActionResult Logout()
        {


            HttpContext.Session.Remove(
                "Admin"
            );





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