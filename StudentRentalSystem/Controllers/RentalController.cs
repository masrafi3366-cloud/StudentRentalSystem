using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models;


namespace StudentRentalSystem.Controllers
{

    public class RentalController : Controller
    {


        private readonly ApplicationDbContext _context;



        public RentalController(ApplicationDbContext context)
        {
            _context = context;
        }





        // =========================
        // RENT PAGE
        // =========================

        public IActionResult Create(int id)
        {


            int? studentId =
            HttpContext.Session.GetInt32("StudentId");



            if (studentId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }



            var item =
            _context.Items
            .FirstOrDefault(
                x => x.ItemId == id
            );



            if (item == null)
            {
                return NotFound();
            }



            return View(item);

        }








        // =========================
        // SAVE RENTAL
        // =========================


        [HttpPost]
        public IActionResult Create(
            int ItemId,
            DateTime StartDate,
            DateTime EndDate
        )
        {


            int? studentId =
            HttpContext.Session.GetInt32("StudentId");



            if (studentId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }





            var item =
            _context.Items
            .FirstOrDefault(
                x => x.ItemId == ItemId
            );



            if (item == null)
            {
                return NotFound();
            }





            int rentalDays =
            (EndDate - StartDate).Days;



            if (rentalDays <= 0)
            {
                rentalDays = 1;
            }





            decimal totalAmount =
            rentalDays * item.PricePerDay;





            Rental rental = new Rental();



            rental.StudentId =
            studentId.Value;



            rental.ItemId =
            ItemId;



            rental.RentalDays =
            rentalDays;



            rental.StartDate =
            StartDate;



            rental.EndDate =
            EndDate;



            rental.TotalAmount =
            totalAmount;



            rental.Status =
            "Pending";





            _context.Rentals.Add(rental);


            _context.SaveChanges();





            return RedirectToAction(
                "Confirmation",
                new
                {
                    id = rental.RentalId
                }
            );


        }









        // =========================
        // CONFIRMATION PAGE
        // =========================


        public IActionResult Confirmation(int id)
        {


            var rental =
            _context.Rentals
            .FirstOrDefault(
                x => x.RentalId == id
            );



            if (rental == null)
            {
                return NotFound();
            }



            return View(rental);

        }








        // =========================
        // MY RENT ITEMS
        // =========================


        public IActionResult MyRentals()
        {


            int? studentId =
            HttpContext.Session.GetInt32(
                "StudentId"
            );



            if (studentId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }





            var rentals =
            _context.Rentals
            .Where(
                x => x.StudentId == studentId
            )
            .ToList();





            return View(rentals);


        }




    }

}