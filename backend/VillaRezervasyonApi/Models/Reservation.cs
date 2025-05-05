using System.ComponentModel.DataAnnotations;

namespace VillaRezervasyonApi.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        
        [Required]
        public int VillaId { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        public string Status { get; set; }
        
        public Villa Villa { get; set; }
    }
} 