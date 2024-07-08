using OPL_grafana_meilisearch.DTOs;
using OPL_grafana_meilisearch.src.Core.Interface;
using OPL_grafana_meilisearch.src.Infrastructure.Interface;

namespace OPL_grafana_meilisearch.src.Core.Service
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
         public async Task<List<FailedDto>> AddFailedAsync(string username, string password)
        {
            try
            {
                // Simulate async operation
                await Task.Delay(100);

            var result = new List<FailedDto>
            {
                new FailedDto
                {
                    Username = username,
                    Password = password
                }
            };

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
