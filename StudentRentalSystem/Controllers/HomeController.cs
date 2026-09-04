using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Data;
using StudentRentalSystem.Models;
using System.Diagnostics;



namespace StudentRentalSystem.Controllers
{

    public class HomeController : Controller
    {


        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;




        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context
        )
        {

            _logger = logger;

            _context = context;

        }









        // =========================
        // HOME PAGE
        // =========================


        public IActionResult Index()
        {


            ViewBag.StudentName =
            HttpContext.Session.GetString(
                "StudentName"
            );





            var items =
            _context.Items
            .OrderByDescending(
                x => x.ItemId
            )
            .Take(12)
            .ToList();






            return View(items);


        }









        // =========================
        // PRIVACY PAGE
        // =========================


        public IActionResult Privacy()
        {

            return View();

        }









        // =========================
        // ERROR PAGE
        // =========================


        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true
        )]


        public IActionResult Error()
        {


            var requestId =
            Activity.Current?.Id
            ??
            HttpContext.TraceIdentifier;






            _logger.LogError(
                "Application error occurred. RequestId: {RequestId}",
                requestId
            );






            ErrorViewModel errorModel =
            new ErrorViewModel
            {

                RequestId = requestId

            };






            return View(errorModel);


        }






    }

}