namespace imageuploading.Models
{
    public class viewProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int Price { get; set; }

        public IFormFile photo { get; set; } = null!;
    }
}
