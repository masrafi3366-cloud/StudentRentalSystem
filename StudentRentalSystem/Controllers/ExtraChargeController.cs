using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models;


namespace StudentRentalSystem.Controllers
{

    public class ExtraChargeController : Controller
    {


        private readonly ApplicationDbContext _context;



        public ExtraChargeController(ApplicationDbContext context)
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
                x => x.StudentId == studentId
            )
            .Select(
                x => x.Charge
            )
            .ToList();





            return View(charges);


        }





    }

}