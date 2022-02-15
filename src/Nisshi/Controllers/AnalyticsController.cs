using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nisshi.Infrastructure.Security;
using Nisshi.Models;
using Nisshi.Requests.Analytics;

namespace Nisshi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes, Roles = "Administrator, User")]
    public class AnalyticsController : BaseNisshiController
    {
        public AnalyticsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("totals")]
        public async Task<SumTotals> GetTotals(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetSumTotals.Query(), cancellationToken);
        }

        [HttpGet("totals/month")]
        public async Task<IEnumerable<TotalsByMonth>> GetTotalsByMonth(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetSumTotalsGroupByMonth.Query(), cancellationToken);
        }

        [HttpGet("totals/catclass")]
        public async Task<IEnumerable<TotalsByCategoryClass>> GetTotalsByCatClass(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetSumTotalsGroupByCatClass.Query(), cancellationToken);
        }

        [HttpGet("totals/instance")]
        public async Task<IEnumerable<TotalsByInstanceType>> GetTotalsByInstanceType(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetSumTotalsGroupByInstance.Query(), cancellationToken);
        }

        [HttpGet("totals/type")]
        public async Task<IEnumerable<TotalsByType>> GetTotalsByType(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetSumTotalsGroupByType.Query(), cancellationToken);
        }

        [HttpGet("currency/landings")]
        public async Task<LandingsApproaches> GetLandingsApproachesPast90Days(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetTotalLandingsApproachesPast90Days.Query(), cancellationToken);
        }
    }
}
