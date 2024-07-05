using Microsoft.EntityFrameworkCore;
using OPL_grafana_meilisearch;
using OPL_grafana_meilisearch.src.Entities;
using OPL_grafana_meilisearch.src.Infrastructure.Interface;


namespace OPL_grafana_meilisearch.src.Infrastructure.Repositories
{
    public class FailedRepository : IFailedRepository
    {
        private readonly DataContext _dbContext;

        public FailedRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async  Task<List<FailedDbo>> GetAllFailedAsync()
        {
            try{
                return await _dbContext.Faileds.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
