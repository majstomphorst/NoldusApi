using Microsoft.EntityFrameworkCore;
using NoldusApi.Models;

namespace NoldusApi.DataAccess
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}