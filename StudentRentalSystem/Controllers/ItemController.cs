using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models;
using StudentRentalSystem.Models.ViewModels;



namespace StudentRentalSystem.Controllers
{

    public class ItemController : Controller
    {


        private readonly ApplicationDbContext _context;



        public ItemController(
            ApplicationDbContext context
        )
        {

            _context = context;

        }








        // =========================
        // CREATE ITEM PAGE
        // =========================


        public IActionResult Create()
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





            ItemCreateViewModel model =
            new ItemCreateViewModel();


            model.Item =
            new Item();




            return View(model);


        }









        // =========================
        // SAVE ITEM
        // =========================


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            ItemCreateViewModel model
        )
        {


            try
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








                if (!ModelState.IsValid)
                {

                    return View(model);

                }







                Item item =
                model.Item;








                // IMAGE UPLOAD


                if (model.Image != null &&
                    model.Image.Length > 0)
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
                        model.Image.FileName
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

                        model.Image.CopyTo(stream);

                    }







                    item.Image =
                    "/uploads/items/" + fileName;


                }









                item.StudentId =
                studentId.Value;



                item.AdminApproved =
                true;



                item.IsRented =
                false;



                item.CreatedDate =
                DateTime.Now;







                _context.Items.Add(item);


                _context.SaveChanges();









                // PAYMENT METHODS SAVE


                if (model.PaymentMethods != null)
                {


                    foreach (var payment in model.PaymentMethods)
                    {


                        if (!string.IsNullOrEmpty(payment.MethodName)
                           &&
                           !string.IsNullOrEmpty(payment.AccountNumber))
                        {


                            PaymentMethod method =
                            new PaymentMethod
                            {


                                ItemId =
                                item.ItemId,


                                MethodName =
                                payment.MethodName,


                                AccountNumber =
                                payment.AccountNumber


                            };




                            _context.PaymentMethods.Add(
                                method
                            );


                        }


                    }



                    _context.SaveChanges();


                }









                TempData["Success"] =
                "Item posted successfully.";







                return RedirectToAction(
                    "MyItems"
                );



            }


            catch (Exception ex)
            {


                Console.WriteLine(
                    ex.Message
                );



                TempData["Error"] =
                "Item posting failed.";





                return View(model);


            }


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
                x =>
                x.StudentId ==
                studentId.Value
            )
            .OrderByDescending(
                x =>
                x.CreatedDate
            )
            .ToList();







            return View(items);


        }









        // =========================
        // BROWSE ITEMS
        // =========================


        public IActionResult Browse(
            string search,
            string category
        )
        {


            var items =
            _context.Items
            .Where(
                x =>
                x.AdminApproved
                &&
                !x.IsRented
            )
            .AsQueryable();







            if (!string.IsNullOrEmpty(search))
            {

                items =
                items.Where(
                    x =>
                    x.ItemName.Contains(search)
                    ||
                    x.Description.Contains(search)
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








            ViewBag.Categories =
            _context.Items
            .Where(
                x =>
                x.AdminApproved
            )
            .Select(
                x =>
                x.Category
            )
            .Distinct()
            .ToList();








            ViewBag.Search =
            search;



            ViewBag.SelectedCategory =
            category;







            return View(
                items
                .OrderByDescending(
                    x =>
                    x.CreatedDate
                )
                .ToList()
            );


        }

        // =========================
        // EDIT ITEM GET
        // =========================


        public IActionResult Edit(int id)
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
                x =>
                x.ItemId == id
                &&
                x.StudentId == studentId.Value
            );








            if (item == null)
            {

                return Unauthorized();

            }








            return View(item);


        }









        // =========================
        // EDIT ITEM POST
        // =========================


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
            Item model,
            IFormFile Image
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
                x =>
                x.ItemId == model.ItemId
                &&
                x.StudentId == studentId.Value
            );








            if (item == null)
            {

                return Unauthorized();

            }








            item.ItemName =
            model.ItemName;



            item.Category =
            model.Category;



            item.Description =
            model.Description;



            item.PricePerDay =
            model.PricePerDay;









            if (Image != null &&
               Image.Length > 0)
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







            _context.SaveChanges();







            TempData["Success"] =
            "Item updated successfully.";








            return RedirectToAction(
                "MyItems"
            );


        }









        // =========================
        // DELETE ITEM
        // =========================


        public IActionResult Delete(int id)
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
                x =>
                x.ItemId == id
                &&
                x.StudentId == studentId.Value
            );








            if (item == null)
            {

                return Unauthorized();

            }








            if (item.IsRented)
            {

                TempData["Error"] =
                "Rented item cannot be deleted.";


                return RedirectToAction(
                    "MyItems"
                );

            }








            _context.Items.Remove(item);


            _context.SaveChanges();








            TempData["Success"] =
            "Item deleted successfully.";








            return RedirectToAction(
                "MyItems"
            );


        }









        // =========================
        // ITEM DETAILS
        // =========================


        public IActionResult Details(int id)
        {


            var item =
            _context.Items
            .FirstOrDefault(
                x =>
                x.ItemId == id
            );








            if (item == null)
            {

                return NotFound();

            }








            var owner =
            _context.Students
            .FirstOrDefault(
                x =>
                x.StudentId == item.StudentId
            );








            if (owner != null)
            {

                ViewBag.OwnerName =
                owner.FullName;



                ViewBag.OwnerMobile =
                owner.Mobile;



                ViewBag.OwnerEmail =
                owner.Email;


            }









            int? currentStudentId =
            HttpContext.Session.GetInt32(
                "StudentId"
            );








            if (currentStudentId != null
               &&
               item.StudentId ==
               currentStudentId.Value)
            {

                ViewBag.IsOwner =
                true;

            }
            else
            {

                ViewBag.IsOwner =
                false;

            }








            ViewBag.IsAvailable =
            !item.IsRented;








            return View(item);


        }




    }

}