using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Models;

namespace Nisshi.Controllers.Interfaces
{
    public interface IVehiclesController
    {
        Task<IEnumerable<Vehicle>> GetAll(CancellationToken cancellationToken);

        Task<Vehicle> GetOne(int id, CancellationToken cancellationToken);

        Task<Vehicle> Update(Vehicle vehicle, CancellationToken cancellationToken);

        Task<Vehicle> Create(Vehicle vehicle, CancellationToken cancellationToken);

        Task<Vehicle> Delete(int id, CancellationToken cancellationToken);
    }
}
