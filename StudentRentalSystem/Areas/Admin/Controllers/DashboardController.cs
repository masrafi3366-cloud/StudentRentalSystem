using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models.ViewModels;



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
        // STUDENT SEARCH
        // =========================


        public IActionResult Index(string search)
        {


            if (HttpContext.Session.GetString("Admin") == null)
            {

                return RedirectToAction(
                    "Login",
                    "Admin"
                );

            }







            var students =
            _context.Students
            .AsQueryable();








            if (!string.IsNullOrEmpty(search))
            {


                students =
                students.Where(
                    x =>
                    x.FullName.Contains(search)
                    ||
                    x.Email.Contains(search)
                    ||
                    x.Mobile.Contains(search)
                );


            }








            var data =
            students
            .OrderByDescending(
                x => x.RegistrationDate
            )
            .ToList();








            // DASHBOARD STATISTICS



            ViewBag.TotalStudents =
            _context.Students.Count();




            ViewBag.ApprovedStudents =
            _context.Students.Count(
                x => x.IsApproved
            );




            ViewBag.PendingStudents =
            _context.Students.Count(
                x => !x.IsApproved
            );






            ViewBag.TotalItems =
            _context.Items.Count();





            ViewBag.AvailableItems =
            _context.Items.Count(
                x =>
                x.AdminApproved
                &&
                !x.IsRented
            );





            ViewBag.RentedItems =
            _context.Items.Count(
                x => x.IsRented
            );





            ViewBag.TotalRentals =
            _context.Rentals.Count();





            ViewBag.CompletedPayments =
            _context.Payments.Count(
                x =>
                x.PaymentStatus == "Completed"
            );





            ViewBag.TotalRevenue =
            _context.Payments
            .Where(
                x =>
                x.PaymentStatus == "Completed"
            )
            .Sum(
                x => x.Amount
            );








            return View(data);


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
        // ITEM SEARCH + CATEGORY FILTER
        // ==================================================



        public IActionResult Items(
            string search,
            string category
        )
        {



            if (HttpContext.Session.GetString("Admin") == null)
            {

                return RedirectToAction(
                    "Login",
                    "Admin"
                );

            }








            var items =
            _context.Items
            .AsQueryable();








            if (!string.IsNullOrEmpty(search))
            {

                items =
                items.Where(
                    x =>
                    x.ItemName.Contains(search)
                );


            }








            if (!string.IsNullOrEmpty(category))
            {

                items =
                items.Where(
                    x =>
                    x.Category == category
                );


            }








            var data =
            items
            .OrderByDescending(
                x => x.CreatedDate
            )
            .ToList();








            foreach (var item in data)
            {


                var owner =
                _context.Students
                .FirstOrDefault(
                    x =>
                    x.StudentId == item.StudentId
                );





                ViewData[
                    "Owner_" + item.ItemId
                ] =
                owner != null
                ?
                owner.FullName + " | " + owner.Mobile
                :
                "Unknown";


            }








            ViewBag.Categories =
            _context.Items
            .Select(
                x => x.Category
            )
            .Distinct()
            .ToList();








            return View(data);



        }









        // =========================
        // ITEM DETAILS
        // =========================


        public IActionResult ItemDetails(int id)
        {


            if (HttpContext.Session.GetString("Admin") == null)
            {

                return RedirectToAction(
                    "Login",
                    "Admin"
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








            ViewBag.Owner =
            _context.Students
            .FirstOrDefault(
                x =>
                x.StudentId == item.StudentId
            );








            return View(item);


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









        // ==================================================
        // RENTAL + PAYMENT MONITORING
        // SEARCH + FILTER
        // ==================================================


        public IActionResult Rentals(
            string search,
            string status
        )
        {


            if (HttpContext.Session.GetString("Admin") == null)
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
            .AsQueryable();








            var data =
            rentals
            .Select(
                rental => new AdminRentalViewModel
                {


                    RentalId =
                    rental.RentalId,






                    StudentName =
                    _context.Students
                    .Where(
                        x =>
                        x.StudentId == rental.StudentId
                    )
                    .Select(
                        x => x.FullName
                    )
                    .FirstOrDefault()
                    ??
                    "Unknown",






                    ItemName =
                    _context.Items
                    .Where(
                        x =>
                        x.ItemId == rental.ItemId
                    )
                    .Select(
                        x => x.ItemName
                    )
                    .FirstOrDefault()
                    ??
                    "Unknown",






                    StartDate =
                    rental.StartDate,






                    EndDate =
                    rental.EndDate,






                    RentalDays =
                    rental.RentalDays,






                    TotalAmount =
                    rental.TotalAmount,






                    RentalStatus =
                    rental.Status,






                    PaymentStatus =
                    _context.Payments
                    .Where(
                        x =>
                        x.RentalId == rental.RentalId
                    )
                    .Select(
                        x => x.PaymentStatus
                    )
                    .FirstOrDefault()
                    ??
                    "Pending",






                    TransactionId =
                    _context.Payments
                    .Where(
                        x =>
                        x.RentalId == rental.RentalId
                    )
                    .Select(
                        x => x.TransactionId
                    )
                    .FirstOrDefault()
                    ??
                    "N/A",






                    PaymentDate =
                    _context.Payments
                    .Where(
                        x =>
                        x.RentalId == rental.RentalId
                    )
                    .Select(
                        x => x.PaymentDate
                    )
                    .FirstOrDefault()



                }
            )
            .ToList();









            if (!string.IsNullOrEmpty(search))
            {

                data =
                data
                .Where(
                    x =>
                    x.StudentName.Contains(search)
                    ||
                    x.ItemName.Contains(search)
                )
                .ToList();


            }








            if (!string.IsNullOrEmpty(status))
            {


                data =
                data
                .Where(
                    x =>
                    x.RentalStatus == status
                )
                .ToList();


            }








            ViewBag.StatusList =
            new List<string>
            {
        "Active",
        "Returned",
        "Late",
        "Pending"
            };








            return View(data);


        }









        // =========================
        // RENTAL DETAILS
        // =========================


        public IActionResult RentalDetails(int id)
        {


            if (HttpContext.Session.GetString("Admin") == null)
            {

                return RedirectToAction(
                    "Login",
                    "Admin"
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








            ViewBag.Student =
            _context.Students
            .FirstOrDefault(
                x =>
                x.StudentId == rental.StudentId
            );








            ViewBag.Item =
            _context.Items
            .FirstOrDefault(
                x =>
                x.ItemId == rental.ItemId
            );








            ViewBag.Payment =
            _context.Payments
            .FirstOrDefault(
                x =>
                x.RentalId == rental.RentalId
            );








            return View(rental);


        }





    }

}