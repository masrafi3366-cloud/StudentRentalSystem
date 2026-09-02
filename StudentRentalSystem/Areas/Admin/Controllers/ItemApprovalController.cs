using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models;


namespace StudentRentalSystem.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ItemApprovalController : Controller
    {


        private readonly ApplicationDbContext _context;



        public ItemApprovalController(ApplicationDbContext context)
        {
            _context = context;
        }





        // =========================
        // ALL ITEMS
        // =========================


        public IActionResult Index()
        {


            var items =
            _context.Items
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







        // =========================
        // APPROVE ITEM
        // =========================


        public IActionResult Approve(int id)
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
                "Index"
            );


        }







        // =========================
        // REJECT ITEM
        // =========================


        public IActionResult Reject(int id)
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
                "Index"
            );


        }



    }

}