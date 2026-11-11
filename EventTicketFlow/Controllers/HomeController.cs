using EventTicketFlow.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EventTicketFlow.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            // Temporary test messages for layout verification
            //TempData["ErrorMessage"] = "This is a test error pop-out! Please check your input.";
            //TempData["SuccessMessage"] = "Your operation was completed successfully!";
            //TempData["WarningMessage"] = "Notice: The venue capacity is reaching its maximum limit.";


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
