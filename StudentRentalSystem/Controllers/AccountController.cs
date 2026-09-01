using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRentalSystem.Data;


namespace StudentRentalSystem.Controllers
{

    public class AccountController : Controller
    {

        private readonly ApplicationDbContext _context;


        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }



        // Login Page

        public IActionResult Login()
        {
            return View();
        }



        // Login Submit


        [HttpPost]
        public IActionResult Login(string email, string password)
        {


            var student = _context.Students
                .FirstOrDefault(
                x => x.Email == email
                &&
                x.Password == password
                );



            if (student == null)
            {

                ViewBag.Error =
                "Invalid email or password";

                return View();

            }



            if (student.IsApproved == false)
            {

                ViewBag.Error =
                "Your account is waiting for admin approval";


                return View();

            }



            HttpContext.Session.SetInt32(
                "StudentId",
                student.StudentId
            );


            HttpContext.Session.SetString(
                "StudentName",
                student.FullName
            );



            return RedirectToAction(
                "Dashboard"
            );

        }





        public IActionResult Dashboard()
        {


            var name =
            HttpContext.Session.GetString(
                "StudentName"
            );


            ViewBag.Name = name;


            return View();

        }





        public IActionResult Logout()
        {


            HttpContext.Session.Clear();


            return RedirectToAction(
                "Login"
            );


        }


    }

}