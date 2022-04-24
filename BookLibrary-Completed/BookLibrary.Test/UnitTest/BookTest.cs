using System;
using System.Collections.Generic;
using BookLibrary.Data.DbContexts;
using BookLibrary.Data.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookLibrary.Test.UnitTest
{
    public class BookTest
    {
        [Fact]
        public void Get_All_Books()
        {
            var serviceProvider = BuildServiceProvider();
            var bookService = serviceProvider.GetRequiredService<BookService>();

            var result = bookService.GetBooks();

            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }

        [Fact]
        public void Get_1_Book()
        {
            var serviceProvider = BuildServiceProvider();
            var bookService = serviceProvider.GetRequiredService<BookService>();

            var id = Guid.NewGuid();
            var result = bookService.GetBook(id);

            Assert.False(result.Success);
        }

        [Fact]
        public void Create_Book()
        {
            var serviceProvider = BuildServiceProvider();
            var bookService = serviceProvider.GetRequiredService<BookService>();

            var id = Guid.NewGuid();
            var result = bookService.CreateBook(null, null, "TITLE", "Author","publishedBy", "Description", 1900, new List<Guid>());
            Assert.False(result.Success);
        }


        [Fact]
        public void Update_Book()
        {
            var serviceProvider = BuildServiceProvider();
            var bookService = serviceProvider.GetRequiredService<BookService>();

            var id = Guid.NewGuid();
            var result = bookService.UpdateBook(id, null, null, "TITLE", "Author", "publishedBy", "Description", 1900, new List<Guid>());
            Assert.False(result.Success);
        }


        private static IServiceProvider BuildServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<BookService>();
            services.AddDbContext<AppDbContext>(b => b.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            return services.BuildServiceProvider();
        }
    }
}
