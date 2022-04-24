using BookLibrary.Data.Common;
using BookLibrary.Data.Entity;
using BookLibrary.Data.Service;
using BookLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult Index(BooksVm vm)
        {
            var result = _bookService.GetBooks(vm.Title, vm.Author, vm.PublishedBy, vm.PublishedYear, vm.CategoryId);

            if (!result.Success || result.Data == null)
            {
                return BadRequest();
            }
            var categories = _bookService.GetCategories();
            vm.Categories = categories;
            vm.Books = result.Data;
            return View(vm);
        }

        [HttpGet]
        public IActionResult Detail(Guid id)
        {
            var result = _bookService.GetBook(id);

            if (!result.Success || result.Data == null)
            {
                return BadRequest();
            }

            _bookService.IncreaseViewCount(id);

            var book = result.Data;

            return View(book);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var categories = _bookService.GetCategories();

            var vm = new UpdateBookVm
            {
                Categories = categories
            };

            return View("Update", vm);
        }
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            var categories = _bookService.GetCategories();
            var result = _bookService.GetBook(id);

            if (!result.Success || result.Data == null)
            {
                return BadRequest();
            }

            var book = result.Data;

            var vm = new UpdateBookVm
            {
                Id = book.Id,
                Title = book.Title,
                PublishedBy = book.PublishedBy,
                PublishedYear = book.PublishedYear,
                Description = book.Description,
                Author = book.Author,
                CategoryIds = book.Categories.Select(c => c.Id).ToList(),
                Categories = categories,
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Download(Guid id)
        {
            var result = _bookService.GetBook(id);

            if (!result.Success || result.Data == null)
            {
                return BadRequest();
            }

            _bookService.IncreaseDownloadCount(id);
            var book = result.Data;
            var data = System.IO.File.ReadAllBytes(book.PdfPath);
            return File(data,"appplication/pdf",$"{book.Title}.pdf");
        }

        [HttpGet]
        public IActionResult Image(Guid id)
        {
            var result = _bookService.GetBook(id);

            if (!result.Success || result.Data == null)
            {
                return BadRequest();
            }

            var book = result.Data;
            var data = System.IO.File.ReadAllBytes(book.ImagePath);
            return File(data, "image/jpg", $"{book.Title}.jpg");
        }

        [HttpPost]
        public IActionResult Update(UpdateBookVm vm)
        {
            if (vm.Id != Guid.Empty)
            {
                ModelState.Remove("PdfFile");
                ModelState.Remove("ImageFile");
            }

            if (!ModelState.IsValid)
            {
                var categories = _bookService.GetCategories();
                vm.Categories = categories;
                return View(vm);
            }

            Result<Book> result;

            if (vm.Id == Guid.Empty)
            {
                result = _bookService.CreateBook(vm.PdfFile, vm.ImageFile,vm.Title, vm.Author, vm.PublishedBy, vm.Description, vm.PublishedYear,
                    vm.CategoryIds);
            }
            else
            {
                result = _bookService.UpdateBook(vm.Id, vm.NewPdfFile, vm.NewImageFile, vm.Title, vm.Author, vm.PublishedBy, vm.Description, vm.PublishedYear,
                    vm.CategoryIds);
            }
            

            if (!result.Success)
            {
                var categories = _bookService.GetCategories();
                vm.Categories = categories;
                this.AddMessage(result.Message);
                return View(vm);
            }

            return RedirectToAction("Detail",new {Id = result.Data.Id});
        }
    }
}
