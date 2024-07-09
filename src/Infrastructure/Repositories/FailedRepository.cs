using Microsoft.EntityFrameworkCore;
using OPL_grafana_meilisearch.src.Entities;
using OPL_grafana_meilisearch.src.Infrastructure.Interface;

using System.Diagnostics;



namespace OPL_grafana_meilisearch.src.Infrastructure.Repositories
{
    public class FailedRepository : IFailedRepository
    {
        private readonly DataContext _dbContext;

        private static readonly ActivitySource activitySource = new ActivitySource("APITracing");


        public FailedRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async  Task<List<FailedDbo>> GetAllFailedAsync()
        {
            using (var activity = activitySource.StartActivity("FailedRepository-GetAllFailedAsync", ActivityKind.Internal))
            {
                activity?.SetTag("component", "FailedRepository");
                activity?.SetTag("internal.service.method", "GetAllFailedAsync");
                
                try
                {
                    return await _dbContext.Faileds.ToListAsync();
                }
                catch (Exception ex)
                {
                    activity?.SetTag("error", true);
                    activity?.SetTag("error.message", ex.Message);
                    activity?.SetTag("error.stacktrace", ex.StackTrace);
                    throw ex;
                }
            }
        }
    }
}
