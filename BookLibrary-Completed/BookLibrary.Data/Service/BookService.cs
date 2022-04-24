using BookLibrary.Data.Common;
using BookLibrary.Data.DbContexts;
using BookLibrary.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace BookLibrary.Data.Service
{
    public class BookService
    {
        private readonly AppDbContext _db;
        public BookService(AppDbContext db)
        {
            _db = db;
            InitCategory();
        }

        public void InitCategory()
        {
            if (_db.Categories.Any())
            {
                return;
            }

            var categories = new List<string>{ "History", "Economic", "Poem", "Comedy", "Comic", "Science", "Health","Education", "Sport" } ;

            foreach (var name in categories)
            {
                var category = new Category
                {
                    CreatedDate = DateTime.Now,
                    Name = name,
                    Description = name,
                };
                _db.Add(category);
            }
            _db.SaveChanges();
        }
        public Result<List<Book>> GetBooks(string? title = default, string? author = default, string? publishedBy = default, int? publishedYear = default, Guid? categoryId = default)
        {
            title = title?.ToLower().Trim();
            List<Guid> bookIds = new();
            if (categoryId.HasValue)
            {
                var category = _db.Categories.Include(c => c.Books)
                    .FirstOrDefault(c => c.Id == categoryId);
                if (category != null)
                {
                    bookIds = category.Books.Select(c => c.Id).ToList();
                }
            }

            var books = _db.Books
                .WhereIf(!string.IsNullOrEmpty(title), b => b.Title.ToLower().Contains(title))
                .WhereIf(!string.IsNullOrEmpty(author), b => b.Author.ToLower().Contains(author))
                .WhereIf(!string.IsNullOrEmpty(publishedBy), b => b.PublishedBy.ToLower().Contains(publishedBy))
                .WhereIf(publishedYear.HasValue, b => b.PublishedYear == publishedYear)
                .WhereIf(categoryId.HasValue,b =>  bookIds.Contains(b.Id))
                .Include(book => book.Categories)
                .OrderByDescending(b => b.CreatedDate)
                .ToList();

            return new(books);
        }

        public Result<Book> CreateBook(IFormFile? pdfFile, IFormFile? imageFile, string? title, string? author, string? publishedBy, string? description, int publishedYear, List<Guid> categoryIds)
        {
            if (pdfFile == null)
            {
                return new("Pdf file is required");
            }

            if (imageFile == null)
            {
                return new("Image file is required");
            }

            if (!pdfFile.IsPdfFile())
            {
                return new("Invalid book file format");
            }

            if (!imageFile.IsJpegImageFile())
            {
                return new("Invalid book image format");
            }

            var pdFileData = pdfFile.GetFileData();
            var pdfFilePath = $"{AppContext.BaseDirectory}/Files/{Guid.NewGuid()}.pdf";

            if (!Directory.Exists($"{AppContext.BaseDirectory}/Files"))
            {
                Directory.CreateDirectory($"{AppContext.BaseDirectory}/Files");
            }
            File.WriteAllBytes(pdfFilePath, pdFileData);

            var imageFileData = imageFile.GetFileData();
            var imageFilePath = $"{AppContext.BaseDirectory}/Files/{Guid.NewGuid()}.jpg";

            if (!Directory.Exists($"{AppContext.BaseDirectory}/Files"))
            {
                Directory.CreateDirectory($"{AppContext.BaseDirectory}/Files");
            }

            File.WriteAllBytes(imageFilePath, imageFileData);

            var book = new Book
            {
                ViewCount = 0,
                DownloadCount = 0,
                CreatedDate = DateTime.Now,
                Title = title,
                Author = author,
                Description = description,
                ImagePath = imageFilePath,
                PdfPath = pdfFilePath,
                PublishedBy = publishedBy,
                PublishedYear = publishedYear,
            };

            var categories = _db.Categories.Where(c => categoryIds.Contains(c.Id)).ToList();
            book.Categories = categories;

            _db.Add(book);
            _db.SaveChanges();

            return new(book);
        }

        public Result<Book> UpdateBook(Guid id, IFormFile? pdfFile, IFormFile? imageFile, string? title, string? author, string? publishedBy, string? description, int publishedYear, List<Guid> categoryIds)
        {
            if (pdfFile != null && !pdfFile.IsPdfFile())
            {
                return new("Invalid book file format");
            }

            if (imageFile != null && !imageFile.IsJpegImageFile())
            {
                return new("Invalid book image format");
            }

            var book = _db.Books
                .Include(b => b.Categories)
                .FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return new("Book not found");
            }

            if (pdfFile != null)
            {
                var pdFileData = pdfFile.GetFileData();
                var pdfFilePath = $"{AppContext.BaseDirectory}/Files/{Guid.NewGuid()}.pdf";
                File.WriteAllBytes(pdfFilePath, pdFileData);
                book.PdfPath = pdfFilePath;
            }

            if (imageFile != null)
            {
                var imageFileData = imageFile.GetFileData();
                var imageFilePath = $"{AppContext.BaseDirectory}/Files/{Guid.NewGuid()}.jpg";
                File.WriteAllBytes(imageFilePath, imageFileData);
                book.ImagePath = imageFilePath;
            }

            book.Title = title;
            book.Author = author;
            book.Description = description;
            book.PublishedBy = publishedBy;
            book.PublishedYear = publishedYear;

            var categories = _db.Categories.Where(c=> categoryIds.Contains(c.Id)).ToList();

            book.Categories = categories;

            _db.Update(book);
            _db.SaveChanges();

            return new(book);
        }

        public List<Category> GetCategories()
        {
            var categories = _db.Categories.ToList();
            return categories;
        }

        public Result<Book> GetBook(Guid id)
        {
            var book = _db.Books
                .Include(b => b.Categories)
                .FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return new("Book not found");
            }

            return new(book);
        }

        public void IncreaseViewCount(Guid id)
        {
            var book = _db.Books.Find(id);
            if (book != null)
            {
                book.ViewCount += 1;
                _db.Update(book);
            }
            _db.SaveChanges();
        }
        public void IncreaseDownloadCount(Guid id)
        {
            var book = _db.Books.Find(id);
            if (book != null)
            {
                book.DownloadCount += 1;
                _db.Update(book);
            }
            _db.SaveChanges();
        }
    }
}
