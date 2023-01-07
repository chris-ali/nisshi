using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Models;

namespace Nisshi.Controllers.Interfaces
{
    public interface IAirportsController
    {
        Task<IEnumerable<Airport>> GetManyByPartialCode(string partialCode, CancellationToken cancellationToken);

        Task<Airport> Create(Airport aircraft, CancellationToken cancellationToken);
    }
}
