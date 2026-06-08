using Microsoft.EntityFrameworkCore;
using ModelLayer.Entities;

namespace RepositoryLayer.Context
{
    public class FundooContext : DbContext
    {
        public FundooContext(
            DbContextOptions<FundooContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}