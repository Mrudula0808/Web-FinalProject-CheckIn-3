namespace BookLibrary.Data.Entity
{
    public class Category
    {
        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Book> Books { get; set; } = new();
    }
}
