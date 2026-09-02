using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models;


namespace StudentRentalSystem.Controllers
{

    public class AccountController : Controller
    {

        private readonly ApplicationDbContext _context;



        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }



        public IActionResult Dashboard()
        {

            int? studentId =
            HttpContext.Session.GetInt32("StudentId");


            if (studentId == null)
            {
                return RedirectToAction("Login");
            }



            var student =
            _context.Students
            .FirstOrDefault(
                x => x.StudentId == studentId
            );


            return View(student);

        }





        public IActionResult Profile()
        {


            int? studentId =
            HttpContext.Session.GetInt32("StudentId");


            if (studentId == null)
            {
                return RedirectToAction("Login");
            }



            var student =
            _context.Students
            .FirstOrDefault(
                x => x.StudentId == studentId
            );



            return View(student);

        }



        public IActionResult Logout()
        {

            HttpContext.Session.Clear();


            return RedirectToAction("Login");

        }


    }

}