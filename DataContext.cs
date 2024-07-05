using Microsoft.EntityFrameworkCore;
using OPL_grafana_meilisearch.src.Entities;

namespace OPL_grafana_meilisearch
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