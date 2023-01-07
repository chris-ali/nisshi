using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Models;

namespace Nisshi.Controllers.Interfaces
{
    public interface IManufacturersController
    {
        Task<IEnumerable<Manufacturer>> GetAll(CancellationToken cancellationToken);

        Task<IEnumerable<Manufacturer>> GetManyByPartialName(string partialName, CancellationToken cancellationToken);

        Task<Manufacturer> Create(Manufacturer manufacturer, CancellationToken cancellationToken);
    }
}
