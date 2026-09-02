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
                x => x.StudentId == studentId
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
            );



            if (rental == null)
            {
                return NotFound();
            }





            DateTime today =
            DateTime.Now;





            if (today <= rental.EndDate)
            {

                rental.Status =
                "Completed";

            }
            else
            {

                rental.Status =
                "Late";



                int lateDays =
                (today - rental.EndDate).Days;



                decimal chargeAmount =
                lateDays * 50;



                ExtraCharge charge =
                new ExtraCharge();



                charge.RentalId =
                rental.RentalId;



                charge.LateDays =
                lateDays;



                charge.Amount =
                chargeAmount;



                charge.PaidStatus =
                false;



                _context.ExtraCharges.Add(charge);

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