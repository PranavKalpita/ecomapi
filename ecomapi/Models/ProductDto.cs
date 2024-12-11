namespace ecomapi.Models
{
    public class ProductDto
    {
        public string Name { get; set; } = "";
        public IFormFile? ImageFile { get; set; }
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public string Unit { get; set; } = "";

        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Time { get; set; } = "";
        public string SellerId { get; set; } = "";
    }
}
