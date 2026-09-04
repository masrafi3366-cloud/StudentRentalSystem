using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models;
using StudentRentalSystem.Helpers;



namespace StudentRentalSystem.Controllers
{

    public class AccountController : Controller
    {


        private readonly ApplicationDbContext _context;



        public AccountController(
            ApplicationDbContext context
        )
        {

            _context = context;

        }








        // =========================
        // REGISTER GET
        // =========================


        public IActionResult Register()
        {

            return View();

        }









        // =========================
        // REGISTER POST
        // =========================


        [HttpPost]
        public IActionResult Register(
            Student student,
            IFormFile StudentIdCardImage
        )
        {



            if (ModelState.IsValid)
            {


                if (StudentIdCardImage != null)
                {


                    string folder =
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/uploads"
                    );





                    if (!Directory.Exists(folder))
                    {

                        Directory.CreateDirectory(folder);

                    }







                    string fileName =
                    Guid.NewGuid().ToString()
                    +
                    Path.GetExtension(
                        StudentIdCardImage.FileName
                    );







                    string filePath =
                    Path.Combine(
                        folder,
                        fileName
                    );







                    using (var stream =
                    new FileStream(
                        filePath,
                        FileMode.Create
                    ))
                    {

                        StudentIdCardImage.CopyTo(stream);

                    }







                    student.StudentIdCardImage =
                    "/uploads/" + fileName;


                }








                student.Password =
                PasswordHelper.HashPassword(
                    student.Password
                );








                student.IsApproved =
                false;







                student.RegistrationDate =
                DateTime.Now;








                _context.Students.Add(student);


                _context.SaveChanges();








                return RedirectToAction(
                    "RegisterSuccess"
                );



            }






            return View(student);


        }









        public IActionResult RegisterSuccess()
        {

            return View();

        }









        // =========================
        // LOGIN GET
        // =========================


        public IActionResult Login()
        {

            return View();

        }









        // =========================
        // LOGIN POST
        // =========================


        [HttpPost]
        public IActionResult Login(
            string email,
            string password
        )
        {


            var student =
            _context.Students
            .FirstOrDefault(
                x =>
                x.Email == email
            );








            if (student == null)
            {

                ViewBag.Error =
                "Invalid email or password";


                return View();

            }








            bool passwordMatch =
            PasswordHelper.VerifyPassword(
                password,
                student.Password
            );








            if (!passwordMatch)
            {

                ViewBag.Error =
                "Invalid email or password";


                return View();

            }








            if (!student.IsApproved)
            {

                ViewBag.Error =
                "Waiting for admin approval";


                return View();

            }








            HttpContext.Session.SetInt32(
                "StudentId",
                student.StudentId
            );







            HttpContext.Session.SetString(
                "StudentName",
                student.FullName
            );








            return RedirectToAction(
                "Index",
                "Home"
            );


        }









        // =========================
        // DASHBOARD
        // =========================


        public IActionResult Dashboard()
        {


            int? studentId =
            HttpContext.Session.GetInt32(
                "StudentId"
            );








            if (studentId == null)
            {

                return RedirectToAction(
                    "Login"
                );

            }








            var student =
            _context.Students
            .FirstOrDefault(
                x =>
                x.StudentId == studentId.Value
            );








            if (student == null)
            {

                return NotFound();

            }








            return View(student);


        }









        // =========================
        // PROFILE
        // =========================


        public IActionResult Profile()
        {


            int? studentId =
            HttpContext.Session.GetInt32(
                "StudentId"
            );








            if (studentId == null)
            {

                return RedirectToAction(
                    "Login"
                );

            }








            var student =
            _context.Students
            .FirstOrDefault(
                x =>
                x.StudentId == studentId.Value
            );








            if (student == null)
            {

                return NotFound();

            }








            ViewBag.PostedItems =
            _context.Items
            .Count(
                x =>
                x.StudentId == studentId.Value
            );








            ViewBag.RentedItems =
            _context.Rentals
            .Count(
                x =>
                x.StudentId == studentId.Value
            );








            ViewBag.Payments =
            _context.Payments
            .Count(
                x =>
                _context.Rentals.Any(
                    r =>
                    r.RentalId == x.RentalId
                    &&
                    r.StudentId == studentId.Value
                )
            );








            ViewBag.ExtraCharges =
            _context.ExtraCharges
            .Count(
                x =>
                _context.Rentals.Any(
                    r =>
                    r.RentalId == x.RentalId
                    &&
                    r.StudentId == studentId.Value
                )
            );








            return View(student);


        }









        // =========================
        // EDIT PROFILE GET
        // =========================


        public IActionResult EditProfile()
        {


            int? studentId =
            HttpContext.Session.GetInt32(
                "StudentId"
            );








            if (studentId == null)
            {

                return RedirectToAction(
                    "Login"
                );

            }








            var student =
            _context.Students
            .FirstOrDefault(
                x =>
                x.StudentId == studentId.Value
            );








            if (student == null)
            {

                return NotFound();

            }








            return View(student);


        }









        // =========================
        // EDIT PROFILE POST
        // =========================


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(
            Student model,
            IFormFile StudentIdCardImage
        )
        {


            int? studentId =
            HttpContext.Session.GetInt32(
                "StudentId"
            );








            if (studentId == null)
            {

                return RedirectToAction(
                    "Login"
                );

            }








            var student =
            _context.Students
            .FirstOrDefault(
                x =>
                x.StudentId == studentId.Value
            );








            if (student == null)
            {

                return NotFound();

            }








            student.FullName =
            model.FullName;







            student.Mobile =
            model.Mobile;







            student.FathersNumber =
            model.FathersNumber;







            student.MothersNumber =
            model.MothersNumber;







            student.Email =
            model.Email;








            if (StudentIdCardImage != null)
            {


                string folder =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/uploads"
                );





                if (!Directory.Exists(folder))
                {

                    Directory.CreateDirectory(folder);

                }





                string fileName =
                Guid.NewGuid().ToString()
                +
                Path.GetExtension(
                    StudentIdCardImage.FileName
                );





                string filePath =
                Path.Combine(
                    folder,
                    fileName
                );





                using (var stream =
                new FileStream(
                    filePath,
                    FileMode.Create
                ))
                {

                    StudentIdCardImage.CopyTo(stream);

                }





                student.StudentIdCardImage =
                "/uploads/" + fileName;


            }



            _context.SaveChanges();







            HttpContext.Session.SetString(
                "StudentName",
                student.FullName
            );








            return RedirectToAction(
                "Profile"
            );


        }









        // =========================
        // CHANGE PASSWORD GET
        // =========================


        public IActionResult ChangePassword()
        {


            int? studentId =
            HttpContext.Session.GetInt32(
                "StudentId"
            );








            if (studentId == null)
            {

                return RedirectToAction(
                    "Login"
                );

            }








            return View();


        }









        // =========================
        // CHANGE PASSWORD POST
        // =========================


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(
            string CurrentPassword,
            string NewPassword,
            string ConfirmPassword
        )
        {


            int? studentId =
            HttpContext.Session.GetInt32(
                "StudentId"
            );








            if (studentId == null)
            {

                return RedirectToAction(
                    "Login"
                );

            }








            var student =
            _context.Students
            .FirstOrDefault(
                x =>
                x.StudentId == studentId.Value
            );








            if (student == null)
            {

                return NotFound();

            }








            bool passwordMatch =
            PasswordHelper.VerifyPassword(
                CurrentPassword,
                student.Password
            );








            if (!passwordMatch)
            {

                ViewBag.Error =
                "Current password is incorrect";


                return View();

            }








            if (NewPassword != ConfirmPassword)
            {

                ViewBag.Error =
                "New password and confirm password do not match";


                return View();

            }








            student.Password =
            PasswordHelper.HashPassword(
                NewPassword
            );








            _context.SaveChanges();








            ViewBag.Success =
            "Password changed successfully";








            return View();


        }









        // =========================
        // LOGOUT
        // =========================


        public IActionResult Logout()
        {


            HttpContext.Session.Clear();




            return RedirectToAction(
                "Login"
            );


        }







    }

}
