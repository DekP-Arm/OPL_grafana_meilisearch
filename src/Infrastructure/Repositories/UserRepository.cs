using Microsoft.EntityFrameworkCore;
using OPL_grafana_meilisearch.src.Entities;
using OPL_grafana_meilisearch.src.Infrastructure.Interface;


namespace OPL_grafana_meilisearch.src.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dbContext;

        public UserRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async  Task<List<UserDbo>> GetAllUserAsync()
        {
            try{
                return await _dbContext.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
