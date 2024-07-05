using Microsoft.EntityFrameworkCore;
using testAPI.src.Entities;
using testAPI.src.Infrastructure.Interface;


namespace testAPI.src.Infrastructure.Repositories
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
