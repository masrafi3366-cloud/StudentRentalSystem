using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models;



namespace StudentRentalSystem.Controllers
{

    public class RentalController : Controller
    {


        private readonly ApplicationDbContext _context;



        public RentalController(
            ApplicationDbContext context
        )
        {

            _context = context;

        }









        // =========================
        // RENT PAGE
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








            if (item.StudentId == studentId.Value)
            {

                TempData["Error"] =
                "You cannot rent your own item.";





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








            var item =
            _context.Items
            .FirstOrDefault(
                x => x.ItemId == ItemId
            );








            if (item == null)
            {

                return NotFound();

            }








            if (item.StudentId == studentId.Value)
            {

                TempData["Error"] =
                "You cannot rent your own item.";





                return RedirectToAction(
                    "Browse",
                    "Item"
                );

            }








            if (item.IsRented)
            {

                TempData["Error"] =
                "Item is already rented.";





                return RedirectToAction(
                    "Browse",
                    "Item"
                );

            }








            if (StartDate.Date < DateTime.Now.Date)
            {

                TempData["Error"] =
                "Start date cannot be in the past.";





                return RedirectToAction(
                    "Details",
                    "Item",
                    new
                    {
                        id = ItemId
                    }
                );

            }








            if (EndDate <= StartDate)
            {

                TempData["Error"] =
                "End date must be after start date.";





                return RedirectToAction(
                    "Details",
                    "Item",
                    new
                    {
                        id = ItemId
                    }
                );

            }








            int rentalDays =
            (EndDate.Date - StartDate.Date).Days;








            if (rentalDays <= 0)
            {

                rentalDays = 1;

            }








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
            rentalDays * item.PricePerDay;








            rental.Status =
            "Confirmed";








            rental.IsReturned =
            false;








            item.IsRented =
            true;








            _context.Rentals.Add(
                rental
            );



            _context.Items.Update(
                item
            );








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







            if (rental.StudentId != studentId.Value)
            {

                return Unauthorized();

            }








            var item =
            _context.Items
            .Include(
                x => x.PaymentMethods
            )
            .FirstOrDefault(
                x => x.ItemId == rental.ItemId
            );








            if (item == null)
            {

                return NotFound();

            }








            var owner =
            _context.Students
            .FirstOrDefault(
                x => x.StudentId == item.StudentId
            );








            var payment =
            _context.Payments
            .FirstOrDefault(
                x => x.RentalId == rental.RentalId
            );








            var data =
            new StudentRentalSystem.Models.ViewModels.RentalConfirmationViewModel
            {

                RentalId =
                rental.RentalId,


                ItemId =
                rental.ItemId,


                ItemName =
                item.ItemName,


                OwnerName =
                owner != null
                ?
                owner.FullName
                :
                "Unknown",


                OwnerMobile =
                owner != null
                ?
                owner.Mobile
                :
                "N/A",


                StartDate =
                rental.StartDate,


                EndDate =
                rental.EndDate,


                RentalDays =
                rental.RentalDays,


                TotalAmount =
                rental.TotalAmount,


                Status =
                rental.Status,


                PaymentStatus =
                payment != null
                ?
                payment.PaymentStatus
                :
                "Pending",



                PaymentMethods =
                item.PaymentMethods.ToList()


            };








            return View(data);


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
                x =>
                x.StudentId == studentId.Value
            )
            .OrderByDescending(
                x => x.StartDate
            )
            .ToList();








            return View(rentals);


        }









        // =========================
        // MY RENTED ITEMS
        // =========================


        public IActionResult MyRentedItems()
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








            var rentedItems =
            _context.Rentals
            .Where(
                x =>
                x.StudentId == studentId.Value
                &&
                x.IsReturned == false
            )
            .OrderByDescending(
                x => x.StartDate
            )
            .ToList();








            return View(rentedItems);


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
                x =>
                x.RentalId == id
                &&
                x.StudentId == studentId.Value
            );








            if (rental == null)
            {

                return NotFound();

            }








            if (rental.IsReturned)
            {

                TempData["Error"] =
                "Item already returned.";





                return RedirectToAction(
                    "MyRentals"
                );

            }








            DateTime today =
            DateTime.Now;








            rental.IsReturned =
            true;








            rental.Status =
            "Returned";








            rental.ReturnDate =
            today;









            // =========================
            // LATE RETURN CHECK
            // =========================


            if (today.Date > rental.EndDate.Date)
            {


                int lateDays =
                (today.Date - rental.EndDate.Date).Days;





                var rentalItem =
                _context.Items
                .FirstOrDefault(
                    x =>
                    x.ItemId == rental.ItemId
                );






                decimal totalCharge =
                0;





                if (rentalItem != null)
                {


                    decimal dailyCharge =
                    rentalItem.PricePerDay * 0.20m;



                    totalCharge =
                    dailyCharge * lateDays;


                }








                ExtraCharge charge =
                new ExtraCharge();








                charge.RentalId =
                rental.RentalId;








                charge.LateDays =
                lateDays;








                charge.Amount =
                totalCharge;








                charge.PaidStatus =
                false;








                _context.ExtraCharges.Add(
                    charge
                );


            }









            // =========================
            // UNLOCK ITEM
            // =========================


            var item =
            _context.Items
            .FirstOrDefault(
                x =>
                x.ItemId == rental.ItemId
            );








            if (item != null)
            {

                item.IsRented =
                false;

            }








            _context.SaveChanges();








            TempData["Success"] =
            "Item returned successfully.";








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
                x =>
                x.RentalId == id
            );








            if (rental == null)
            {

                return NotFound();

            }








            if (rental.StudentId != studentId.Value)
            {

                return Unauthorized();

            }








            return View(rental);


        }





    }

}