using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models;


namespace StudentRentalSystem.Controllers
{

    public class AccountController : Controller
    {

        private readonly ApplicationDbContext _context;



        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }



        // Registration Page

        public IActionResult Register()
        {
            return View();
        }



        // Registration Submit


        [HttpPost]
        public IActionResult Register(Student student, IFormFile StudentIdCardImage)
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
                    FileMode.Create))
                    {

                        StudentIdCardImage.CopyTo(stream);

                    }



                    student.StudentIdCardImage =
                    "/uploads/" + fileName;


                }



                student.IsApproved = false;


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



    }

}