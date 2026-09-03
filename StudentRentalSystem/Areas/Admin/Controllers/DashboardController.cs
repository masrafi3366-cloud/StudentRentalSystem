using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models;


namespace StudentRentalSystem.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class DashboardController : Controller
    {


        private readonly ApplicationDbContext _context;



        public DashboardController(
            ApplicationDbContext context
        )
        {

            _context = context;

        }








        // =========================
        // ADMIN DASHBOARD
        // =========================


        public IActionResult Index()
        {


            if (
                HttpContext.Session.GetString("Admin")
                == null
            )
            {

                return RedirectToAction(
                    "Login",
                    "Admin"
                );

            }







            var students =
            _context.Students
            .OrderByDescending(
                x => x.RegistrationDate
            )
            .ToList();






            return View(students);


        }









        // =========================
        // APPROVE STUDENT
        // =========================


        public IActionResult Approve(int id)
        {


            var student =
            _context.Students
            .FirstOrDefault(
                x => x.StudentId == id
            );







            if (student != null)
            {

                student.IsApproved = true;


                _context.SaveChanges();


            }






            return RedirectToAction(
                "Index"
            );


        }









        // =========================
        // REJECT STUDENT
        // =========================


        public IActionResult Reject(int id)
        {


            var student =
            _context.Students
            .FirstOrDefault(
                x => x.StudentId == id
            );







            if (student != null)
            {


                _context.Students.Remove(student);


                _context.SaveChanges();


            }






            return RedirectToAction(
                "Index"
            );


        }









        // =========================
        // STUDENT DETAILS
        // =========================


        public IActionResult StudentDetails(int id)
        {


            var student =
            _context.Students
            .FirstOrDefault(
                x => x.StudentId == id
            );







            if (student == null)
            {

                return NotFound();

            }







            return View(student);


        }









        // ==================================================
        // ITEM MANAGEMENT
        // ==================================================







        // =========================
        // ALL ITEMS
        // =========================


        public IActionResult Items()
        {


            if (
                HttpContext.Session.GetString("Admin")
                == null
            )
            {

                return RedirectToAction(
                    "Login",
                    "Admin"
                );

            }








            var items =
            _context.Items
            .OrderByDescending(
                x => x.CreatedDate
            )
            .ToList();








            return View(items);


        }









        // =========================
        // APPROVE ITEM
        // =========================


        public IActionResult ApproveItem(int id)
        {


            var item =
            _context.Items
            .FirstOrDefault(
                x => x.ItemId == id
            );







            if (item != null)
            {


                item.AdminApproved = true;


                _context.SaveChanges();


            }







            return RedirectToAction(
                "Items"
            );


        }









        // =========================
        // REJECT ITEM
        // =========================


        public IActionResult RejectItem(int id)
        {


            var item =
            _context.Items
            .FirstOrDefault(
                x => x.ItemId == id
            );







            if (item != null)
            {


                _context.Items.Remove(item);


                _context.SaveChanges();


            }







            return RedirectToAction(
                "Items"
            );


        }


        // =========================
        // RENTAL MONITORING
        // =========================


        public IActionResult Rentals()
        {


            if (
                HttpContext.Session.GetString("Admin")
                == null
            )
            {

                return RedirectToAction(
                    "Login",
                    "Admin"
                );

            }






            var rentals =
            _context.Rentals
            .OrderByDescending(
                x => x.RentalId
            )
            .ToList();






            return View(rentals);


        }




    }

}