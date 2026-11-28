using EventTicketFlow.Data;
using EventTicketFlow.Models;

namespace EventTicketFlow.Services
{
    public class DiscountService
    {
        AppDbContext _context;

        public DiscountService(AppDbContext context)
        {
            _context = context;
        }

        // Checks a code says if and why it fails
        public DiscountResult CheckCode(string? code)
        {
            DiscountResult result = new DiscountResult();

            if (string.IsNullOrWhiteSpace(code))
            {
                result.Message = "Enter a discount code.";
                return result;
            }

            string cleanCode = code.Trim();

            Discount? discount = _context.Discounts.FirstOrDefault(d => d.Code == cleanCode);

            if (discount == null)
            {
                result.Message = "That discount code doesn't exist.";
                return result;
            }

            if (!discount.IsActive)
            {
                result.Message = "That discount code is no longer active.";
                return result;
            }

            if (discount.ExpirationDate.HasValue && discount.ExpirationDate.Value.Date < DateTime.Today)
            {
                result.Message = "That discount code expired on " + discount.ExpirationDate.Value.ToString("MMM d, yyyy") + ".";
                return result;
            }

            if (discount.MaxUses.HasValue && discount.TimesUsed >= discount.MaxUses.Value)
            {
                result.Message = "That discount code has been used already.";
                return result;
            }

            result.IsValid = true;
            result.PercentOff = discount.PercentOff;
            result.Message = discount.PercentOff + "% off applied.";
            return result;
        }

        // Takes the percent off a total rounded up
        public static decimal ApplyDiscount(decimal total, int percentOff)
        {
            decimal discounted = total - (total * percentOff / 100m);
            return Math.Round(discounted, 2, MidpointRounding.AwayFromZero);
        }
    }
}