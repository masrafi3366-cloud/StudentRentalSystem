using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models;


namespace StudentRentalSystem.Controllers
{

    public class ItemController : Controller
    {


        private readonly ApplicationDbContext _context;



        public ItemController(ApplicationDbContext context)
        {
            _context = context;
        }






        // =========================
        // CREATE ITEM PAGE
        // =========================


        public IActionResult Create()
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


            return View();

        }







        // =========================
        // SAVE ITEM
        // =========================


        [HttpPost]
        public IActionResult Create(Item item, IFormFile Image)
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





            if (ModelState.IsValid)
            {



                if (Image != null)
                {


                    string folder =
                    Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/uploads/items"
                    );



                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }



                    string fileName =
                    Guid.NewGuid().ToString()
                    +
                    Path.GetExtension(
                        Image.FileName
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

                        Image.CopyTo(stream);

                    }




                    item.Image =
                    "/uploads/items/" + fileName;


                }



                item.StudentId =
                studentId.Value;



                item.AdminApproved = false;


                item.CreatedDate =
                DateTime.Now;



                _context.Items.Add(item);


                _context.SaveChanges();



                return RedirectToAction(
                    "MyItems"
                );

            }



            return View(item);

        }








        // =========================
        // MY ITEMS
        // =========================


        public IActionResult MyItems()
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




            var items =
            _context.Items
            .Where(
                x => x.StudentId == studentId
            )
            .ToList();



            return View(items);

        }









        // =========================
        // BROWSE ITEMS
        // =========================


        public IActionResult Browse()
        {


            var items =
            _context.Items
            .Where(
                x => x.AdminApproved == true
            )
            .ToList();



            return View(items);

        }







        // =========================
        // ITEM DETAILS
        // =========================


        public IActionResult Details(int id)
        {


            var item =
            _context.Items
            .FirstOrDefault(
                x => x.ItemId == id
            );



            return View(item);

        }



    }

}