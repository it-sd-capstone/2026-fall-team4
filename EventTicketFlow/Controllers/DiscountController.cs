using EventTicketFlow.Models;
using EventTicketFlow.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventTicketFlow.Controllers
{
    public class DiscountController : Controller
    {
        private readonly DiscountService _discountService;

        public DiscountController(DiscountService discountService)
        {
            _discountService = discountService;
        }

        // The event page's Apply button calls this and reads the JSON it sends back
        [HttpGet]
        public IActionResult Check(string? code)
        {
            DiscountResult result = _discountService.CheckCode(code);
            return Json(result);
        }
    }
}