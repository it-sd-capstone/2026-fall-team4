using System.ComponentModel.DataAnnotations;

namespace EventTicketFlow.Models
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Range(1, 100)]
        [Display(Name = "Percent Off")]
        public int PercentOff { get; set; }

        [Display(Name = "Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Max Uses")]
        public int? MaxUses { get; set; }

        [Display(Name = "Times Used")]
        public int TimesUsed { get; set; }

    }
}
