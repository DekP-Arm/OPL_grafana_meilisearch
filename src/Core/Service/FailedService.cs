using OPL_grafana_meilisearch.DTOs;
using OPL_grafana_meilisearch.src.Core.Interface;
using OPL_grafana_meilisearch.src.Infrastructure.Interface;

using System.Diagnostics;

namespace OPL_grafana_meilisearch.src.Core.Service
{
    public class FailedService : IFailedService
    {
        public readonly  IFailedRepository _failedRepository;

        private static readonly ActivitySource activitySource = new ActivitySource("APITracing");

        public FailedService(IFailedRepository failedRepository)
        {
            _failedRepository = failedRepository;
        }

        public async Task<List<FailedDto>> GetAllFailedAsync()
        {
            using (var activity = activitySource.StartActivity("FailedService-GetAllFailedAsync", ActivityKind.Internal))
            {
                activity?.SetTag("component", "FailedService");
                try
                {
                    using (var activityRepo = activitySource.StartActivity("Call Failed Repository - GetAllFailedAsync", ActivityKind.Internal))
                    {
                        activityRepo?.SetTag("internal.service.name", "FailedRepository");
                        activityRepo?.SetTag("internal.service.method", "GetAllFailedAsync");
                        
                        var failedData = await _failedRepository.GetAllFailedAsync();
                        var result = failedData.Select(x => new FailedDto
                        {
                            Username = x.Username
                        }).ToList();
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    activity?.SetTag("error", true);
                    activity?.SetTag("error.message", ex.Message);
                    activity?.SetTag("error.stacktrace", ex.StackTrace);
                    throw;
                }
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
