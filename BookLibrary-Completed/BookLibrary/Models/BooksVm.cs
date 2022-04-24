using System.Collections;
using System.ComponentModel.DataAnnotations;
using BookLibrary.Data.Entity;

namespace BookLibrary.Models
{
    public class BooksVm
    {
        public List<Book> Books { get; set; } = new();
        public string? Title { get; set; }
        public string? Author { get; set; }
        [Display(Name = "Published by")]
        public string? PublishedBy { get; set; }
        [Display(Name = "Published year")]
        public int? PublishedYear { get; set; }
        [Display(Name = "Category")]
        public Guid? CategoryId { get; set; }
        public List<Category> Categories { get; set; } = new();
    }
}
