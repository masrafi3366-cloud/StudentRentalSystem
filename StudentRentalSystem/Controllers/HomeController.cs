using Microsoft.AspNetCore.Mvc;
using StudentRentalSystem.Models;
using System.Diagnostics;


namespace StudentRentalSystem.Controllers
{

    public class HomeController : Controller
    {


        private readonly ILogger<HomeController> _logger;



        public HomeController(
            ILogger<HomeController> logger
        )
        {
            _logger = logger;
        }







        // =========================
        // HOME PAGE
        // =========================


        public IActionResult Index()
        {
            return View();
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


            var errorModel =
            new ErrorViewModel
            {

                RequestId =
                Activity.Current?.Id
                ??
                HttpContext.TraceIdentifier

            };



            _logger.LogError(
                "Application error occurred. RequestId: {RequestId}",
                errorModel.RequestId
            );



            return View(errorModel);

        }





    }

}