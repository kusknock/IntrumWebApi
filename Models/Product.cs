namespace IntrumWebApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? ImageId { get; set; }
        public double? Price { get; set; } = 0;
    }
}
