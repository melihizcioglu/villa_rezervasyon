using System.ComponentModel.DataAnnotations;

namespace VillaRezervasyonApi.Models
{
    public class Villa
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public string Location { get; set; }
        
        [Required]
        public string ImageUrl { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        
        public List<string> Features { get; set; }
    }
} 