using BookLibrary.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Data.DbContexts
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
    }
}