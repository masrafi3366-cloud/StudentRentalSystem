using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models;


namespace StudentRentalSystem.Controllers
{

    public class ExtraChargeController : Controller
    {


        private readonly ApplicationDbContext _context;



        public ExtraChargeController(
            ApplicationDbContext context
        )
        {

            _context = context;

        }









        // =========================
        // MY CHARGES
        // =========================


        public IActionResult MyCharges()
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








            var charges =
            _context.ExtraCharges
            .Join(
                _context.Rentals,
                charge => charge.RentalId,
                rental => rental.RentalId,
                (charge, rental) => new
                {

                    Charge = charge,

                    StudentId = rental.StudentId

                }
            )
            .Where(
                x =>
                x.StudentId == studentId.Value
            )
            .Select(
                x => x.Charge
            )
            .OrderByDescending(
                x => x.ChargeId
            )
            .ToList();








            return View(charges);


        }









        // =========================
        // PAY PAGE (GET)
        // =========================


        [HttpGet]
        public IActionResult Pay(int id)
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








            var charge =
            _context.ExtraCharges
            .FirstOrDefault(
                x =>
                x.ChargeId == id
            );








            if (charge == null)
            {

                return NotFound();

            }








            var rental =
            _context.Rentals
            .FirstOrDefault(
                x =>
                x.RentalId == charge.RentalId
            );








            if (rental == null)
            {

                return NotFound();

            }








            if (rental.StudentId != studentId.Value)
            {

                return Unauthorized();

            }








            if (charge.PaidStatus)
            {

                TempData["Error"] =
                "This charge is already paid.";





                return RedirectToAction(
                    "MyCharges"
                );

            }








            return View(charge);


        }









        // =========================
        // PROCESS PAYMENT (POST)
        // =========================


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessPayment(
            int ChargeId
        )
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








            var charge =
            _context.ExtraCharges
            .FirstOrDefault(
                x =>
                x.ChargeId == ChargeId
            );








            if (charge == null)
            {

                return NotFound();

            }








            var rental =
            _context.Rentals
            .FirstOrDefault(
                x =>
                x.RentalId == charge.RentalId
            );








            if (rental == null)
            {

                return NotFound();

            }








            if (rental.StudentId != studentId.Value)
            {

                return Unauthorized();

            }








            if (charge.PaidStatus)
            {

                TempData["Error"] =
                "Charge already paid.";





                return RedirectToAction(
                    "MyCharges"
                );

            }








            charge.PaidStatus =
            true;








            _context.SaveChanges();








            TempData["Success"] =
            "Extra charge paid successfully.";








            return RedirectToAction(
                "Success",
                new
                {
                    id =
                    charge.ChargeId
                }
            );


        }









        // =========================
        // PAYMENT SUCCESS
        // =========================


        public IActionResult Success(int id)
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








            var charge =
            _context.ExtraCharges
            .FirstOrDefault(
                x =>
                x.ChargeId == id
            );








            if (charge == null)
            {

                return NotFound();

            }








            var rental =
            _context.Rentals
            .FirstOrDefault(
                x =>
                x.RentalId == charge.RentalId
            );








            if (rental == null)
            {

                return NotFound();

            }








            if (rental.StudentId != studentId.Value)
            {

                return Unauthorized();

            }








            return View(charge);


        }





    }

}