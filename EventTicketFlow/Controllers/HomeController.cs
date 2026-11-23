using EventTicketFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using EventTicketFlow.Services;

namespace EventTicketFlow.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly EventService _eventService;

        public HomeController(ILogger<HomeController> logger, EventService eventService)
        {
            _logger = logger;
            _eventService = eventService;
        }

        public IActionResult Index(string? searchTerm, string? status, string? price, string? sort)
        {
            HomeViewModel model = new();

            model.SearchTerm = searchTerm;
            model.Status = status;
            model.Price = price;
            model.Sort = sort;

            model.Events = _eventService.SearchEvents(searchTerm, status, price, sort);

            return View(model);
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

        [Authorize(Roles = "Admin")]
        public IActionResult SecureTest()
        {
            return Content("Admin access granted.");
        }
    }
}