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








            if (item.IsRented)
            {

                TempData["Error"] =
                "This item is already rented.";

                return RedirectToAction(
                    "Browse",
                    "Item"
                );

            }








            return View(item);


        }









        // =========================
        // SAVE RENTAL
        // =========================


        [HttpPost]
        [ValidateAntiForgeryToken]
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








            // CHECK ITEM STATUS


            if (item.IsRented)
            {

                TempData["Error"] =
                "Sorry, this item is already rented.";

                return RedirectToAction(
                    "Browse",
                    "Item"
                );

            }








            int rentalDays =
            (EndDate - StartDate).Days;





            if (rentalDays <= 0)
            {

                rentalDays = 1;

            }







            decimal totalAmount =
            rentalDays * item.PricePerDay;








            Rental rental =
            new Rental();







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
            "Active";









            // LOCK ITEM


            item.IsRented = true;







            _context.Rentals.Add(rental);



            _context.Items.Update(item);




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
        // MY RENTALS
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
                x => x.StudentId == studentId.Value
            )
            .OrderByDescending(
                x => x.StartDate
            )
            .ToList();







            return View(rentals);



        }









        // =========================
        // RETURN ITEM
        // =========================


        public IActionResult ReturnItem(int id)
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








            var rental =
            _context.Rentals
            .FirstOrDefault(
                x => x.RentalId == id
                &&
                x.StudentId == studentId.Value
            );







            if (rental == null)
            {

                return NotFound();

            }








            var item =
            _context.Items
            .FirstOrDefault(
                x => x.ItemId == rental.ItemId
            );








            rental.ReturnDate =
            DateTime.Now;







            rental.Status =
            "Returned";








            // UNLOCK ITEM


            if (item != null)
            {

                item.IsRented = false;

            }









            _context.SaveChanges();







            return RedirectToAction(
                "ReturnSuccess",
                new
                {
                    id = rental.RentalId
                }
            );



        }









        // =========================
        // RETURN SUCCESS
        // =========================


        public IActionResult ReturnSuccess(int id)
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




    }

}