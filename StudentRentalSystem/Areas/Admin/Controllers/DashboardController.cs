using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRentalSystem.Data;


namespace StudentRentalSystem.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class DashboardController : Controller
    {


        private readonly ApplicationDbContext _context;



        public DashboardController(ApplicationDbContext context)
        {

            _context = context;

        }



        public IActionResult Index()
        {

            var students =
            _context.Students
            .ToList();


            return View(students);

        }



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



        public IActionResult StudentDetails(int id)
        {

            var student =
            _context.Students
            .FirstOrDefault(
                x => x.StudentId == id
            );


            return View(student);

        }



    }

}