using System.ComponentModel.DataAnnotations;

namespace EventTicketFlow.Models
{
    public class Settings
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }

    }
}