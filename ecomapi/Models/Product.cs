using System.ComponentModel.DataAnnotations;

namespace ecomapi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Imagefile {  get; set; } = "";
        public string Description { get; set; } = "";
        public string Category {  get; set; } = ""; 
        public string Unit { get; set; } = "";
        
        public int Price { get; set; } 
        public int Quantity { get; set; }
        public string Time {  get; set; } = "";
        public string SellerId { get; set; } = "";
    }
}
