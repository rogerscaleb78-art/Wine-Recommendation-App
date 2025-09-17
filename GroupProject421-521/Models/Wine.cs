using System.ComponentModel.DataAnnotations;

namespace GroupProject421_521.Models
{
    public class Wine
    {
       
        public int WineID { get; set; } 


        public string Name { get; set; } 

  
        public string? Type { get; set; } 

       
        public string? Region { get; set; } 

       
        public string? Description { get; set; }

        public string? FlavorProfile { get; set; }
       
        public decimal Price { get; set; }
    }
}
