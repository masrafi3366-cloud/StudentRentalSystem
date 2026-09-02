using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models;


namespace StudentRentalSystem.Controllers
{

    public class PaymentController : Controller
    {


        private readonly ApplicationDbContext _context;



        public PaymentController(ApplicationDbContext context)
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



            return View(rental);

        }









        // =========================
        // SAVE PAYMENT
        // =========================


        [HttpPost]
        public IActionResult Create(
            int RentalId,
            string TransactionId
        )
        {



            var rental =
            _context.Rentals
            .FirstOrDefault(
                x => x.RentalId == RentalId
            );



            if (rental == null)
            {
                return NotFound();
            }







            Payment payment = new Payment();



            payment.RentalId =
            RentalId;



            payment.Amount =
            rental.TotalAmount;



            payment.PaymentDate =
            DateTime.Now;



            payment.PaymentStatus =
            "Completed";



            payment.TransactionId =
            TransactionId;





            _context.Payments.Add(payment);






            rental.Status =
            "Confirmed";





            _context.SaveChanges();





            return RedirectToAction(
                "Success",
                new
                {
                    id = payment.PaymentId
                }
            );

        }









        // =========================
        // SUCCESS PAGE
        // =========================


        public IActionResult Success(int id)
        {


            var payment =
            _context.Payments
            .FirstOrDefault(
                x => x.PaymentId == id
            );



            if (payment == null)
            {
                return NotFound();
            }



            return View(payment);

        }



    }

}