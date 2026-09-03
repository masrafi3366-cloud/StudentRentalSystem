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



            return View(new Item());

        }









        // =========================
        // SAVE ITEM
        // =========================


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            Item item,
            IFormFile Image
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







            if (!ModelState.IsValid)
            {

                return View(item);

            }








            // IMAGE UPLOAD


            if (Image != null && Image.Length > 0)
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
                    FileMode.Create
                ))
                {

                    Image.CopyTo(stream);

                }






                item.Image =
                "/uploads/items/" + fileName;



            }







            item.StudentId =
            studentId.Value;



            item.AdminApproved =
            false;



            item.CreatedDate =
            DateTime.Now;








            _context.Items.Add(item);


            _context.SaveChanges();






            TempData["Success"] =
            "Item posted successfully. Waiting for admin approval.";







            return RedirectToAction(
                "MyItems"
            );



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
                x => x.StudentId == studentId.Value
            )
            .OrderByDescending(
                x => x.CreatedDate
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
                x => x.AdminApproved
            )
            .OrderByDescending(
                x => x.CreatedDate
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





            if (item == null)
            {

                return NotFound();

            }





            return View(item);


        }




    }

}