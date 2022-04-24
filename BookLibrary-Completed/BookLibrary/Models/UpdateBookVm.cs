using System.ComponentModel.DataAnnotations;
using BookLibrary.Data.Entity;

namespace BookLibrary.Models
{
    public class UpdateBookVm
    {
        [Display(Name = "Book pdf file")]
        [Required]
        public IFormFile? PdfFile { get; set; }
        [Display(Name = "Image")]
        [Required]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "New book pdf file")]
        public IFormFile? NewPdfFile { get; set; }
        [Display(Name = "New image")]
        public IFormFile? NewImageFile { get; set; }
        public Guid Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Display(Name = "Published by")]
        [Required]
        public string? PublishedBy { get; set; }

        [Display(Name = "Published year")]
        [Required]
        public int PublishedYear{ get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        [Display(Name = "Author(s)")]
        public string? Author { get; set; }
        [Display(Name = "Categories")]
        public List<Guid>? CategoryIds { get; set; } = new List<Guid>();
        public List<Category> Categories { get; set; } = new List<Category>();

    }
}
