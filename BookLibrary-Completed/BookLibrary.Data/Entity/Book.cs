namespace BookLibrary.Data.Entity
{
    public class Book
    {
        public Guid Id { get; set; }
        public int ViewCount { get; set; }
        public int DownloadCount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public string? PdfPath { get; set; }
        public string? PublishedBy { get; set; }
        public int PublishedYear { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
