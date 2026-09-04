using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models;



namespace StudentRentalSystem.Controllers
{

    public class PaymentController : Controller
    {


        private readonly ApplicationDbContext _context;



        public PaymentController(
            ApplicationDbContext context
        )
        {

            _context = context;

        }









        // =========================
        // PAYMENT PAGE
        // =========================


        public IActionResult Create(int id)
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








            // USER SECURITY CHECK


            if (rental.StudentId != studentId.Value)
            {

                return Unauthorized();

            }








            // RETURNED RENTAL BLOCK


            if (rental.IsReturned)
            {

                TempData["Error"] =
                "Returned rental cannot be paid.";





                return RedirectToAction(
                    "MyRentals",
                    "Rental"
                );

            }








            return View(rental);


        }









        // =========================
        // SAVE PAYMENT
        // =========================


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            int RentalId,
            string TransactionId
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








            var rental =
            _context.Rentals
            .FirstOrDefault(
                x => x.RentalId == RentalId
            );








            if (rental == null)
            {

                return NotFound();

            }








            if (rental.StudentId != studentId.Value)
            {

                return Unauthorized();

            }








            // TRANSACTION VALIDATION


            if (string.IsNullOrWhiteSpace(TransactionId))
            {

                TempData["Error"] =
                "Transaction ID is required.";





                return RedirectToAction(
                    "Create",
                    new
                    {
                        id = RentalId
                    }
                );

            }








            // DUPLICATE PAYMENT CHECK


            var existingPayment =
            _context.Payments
            .FirstOrDefault(
                x =>
                x.RentalId == RentalId
                &&
                x.PaymentStatus == "Completed"
            );








            if (existingPayment != null)
            {

                return RedirectToAction(
                    "Success",
                    new
                    {
                        id =
                        existingPayment.PaymentId
                    }
                );

            }








            // RETURN CHECK


            if (rental.IsReturned)
            {

                TempData["Error"] =
                "This rental is already returned.";





                return RedirectToAction(
                    "MyRentals",
                    "Rental"
                );

            }








            var item =
            _context.Items
            .FirstOrDefault(
                x =>
                x.ItemId == rental.ItemId
            );








            if (item == null)
            {

                return NotFound();

            }








            Payment payment =
            new Payment();








            payment.RentalId =
            RentalId;








            payment.Amount =
            rental.TotalAmount;








            payment.PaymentDate =
            DateTime.Now;








            payment.PaymentStatus =
            "Completed";








            payment.TransactionId =
            TransactionId.Trim();








            _context.Payments.Add(
                payment
            );








            // CONFIRM RENTAL


            rental.Status =
            "Confirmed";








            // ITEM LOCK


            item.IsRented =
            true;








            _context.SaveChanges();








            return RedirectToAction(
                "Success",
                new
                {
                    id =
                    payment.PaymentId
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








            var payment =
            _context.Payments
            .FirstOrDefault(
                x => x.PaymentId == id
            );








            if (payment == null)
            {

                return NotFound();

            }








            var rental =
            _context.Rentals
            .FirstOrDefault(
                x =>
                x.RentalId == payment.RentalId
            );








            if (rental == null)
            {

                return NotFound();

            }








            // USER SECURITY CHECK


            if (rental.StudentId != studentId.Value)
            {

                return Unauthorized();

            }








            ViewBag.Rental =
            rental;








            return View(payment);


        }









        // =========================
        // MY PAYMENTS
        // =========================


        public IActionResult MyPayments()
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








            var payments =
            _context.Payments
            .Where(
                x =>
                _context.Rentals.Any(
                    r =>
                    r.RentalId == x.RentalId
                    &&
                    r.StudentId == studentId.Value
                )
            )
            .OrderByDescending(
                x => x.PaymentDate
            )
            .ToList();








            return View(payments);


        }






    }

}