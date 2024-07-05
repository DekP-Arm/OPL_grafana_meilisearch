using Microsoft.EntityFrameworkCore;
using testAPI.src.Entities;
using testAPI.src.Infrastructure.Interface;


namespace testAPI.src.Infrastructure.Repositories
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
