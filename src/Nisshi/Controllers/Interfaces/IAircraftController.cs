using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Models;

namespace Nisshi.Controllers.Interfaces
{
    public interface IAircraftController
    {
        Task<IEnumerable<Aircraft>> GetAll(CancellationToken cancellationToken);

        Task<Aircraft> GetOne(int id, CancellationToken cancellationToken);

        Task<Aircraft> Update(Aircraft aircraft, CancellationToken cancellationToken);

        Task<Aircraft> Create(Aircraft aircraft, CancellationToken cancellationToken);

        Task<Aircraft> Delete(int id, CancellationToken cancellationToken);
    }
}
