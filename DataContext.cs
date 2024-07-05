using Microsoft.EntityFrameworkCore;
using testAPI.src.Entities;

namespace testAPI
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<UserDbo> Users {get;set;}
        public DbSet<FailedDbo> Faileds {get;set;}
    }
}