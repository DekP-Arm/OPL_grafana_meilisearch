using testAPI.DTOs;
using testAPI.src.Core.Interface;
using testAPI.src.Infrastructure.Interface;

namespace testAPI.src.Core.Service
{
    public class FailedService : IFailedService
    {
        public readonly  IFailedRepository _failedRepository;

        public FailedService(IFailedRepository failedRepository)
        {
            _failedRepository = failedRepository;
        }

        public async Task<List<FailedDto>> GetAllFailedAsync()
        {
            try{
                var failedData = await _failedRepository.GetAllFailedAsync();
                var result = failedData.Select( x => new FailedDto
                {
                   Username = x.Username
                }
                ).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
