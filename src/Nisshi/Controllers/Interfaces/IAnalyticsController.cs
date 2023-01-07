using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Models;

namespace Nisshi.Controllers.Interfaces
{
    public interface IAnalyticsController
    {
        Task<SumTotals> GetTotals(CancellationToken cancellationToken);

        Task<IEnumerable<TotalsByMonth>> GetTotalsByMonth(CancellationToken cancellationToken);

        Task<IEnumerable<TotalsByCategoryClass>> GetTotalsByCatClass(CancellationToken cancellationToken);

        Task<IEnumerable<TotalsByInstanceType>> GetTotalsByInstanceType(CancellationToken cancellationToken);

        Task<IEnumerable<TotalsByType>> GetTotalsByType(CancellationToken cancellationToken);

        Task<LandingsApproaches> GetLandingsApproachesPast90Days(CancellationToken cancellationToken);
    }
}
