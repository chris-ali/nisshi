using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Models;

namespace Nisshi.Controllers.Interfaces
{
    public interface IMaintenanceEntriesController
    {
        Task<IEnumerable<MaintenanceEntry>> GetAll(CancellationToken cancellationToken);

        Task<MaintenanceEntry> GetOne(int id, CancellationToken cancellationToken);

        Task<MaintenanceEntry> Update(MaintenanceEntry maintenanceEntry, CancellationToken cancellationToken);

        Task<MaintenanceEntry> Create(MaintenanceEntry maintenanceEntry, CancellationToken cancellationToken);

        Task<MaintenanceEntry> Delete(int id, CancellationToken cancellationToken);
    }
}
