using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Models;

namespace Nisshi.Controllers.Interfaces
{
    public interface ILogbookEntriesController
    {
        Task<IEnumerable<LogbookEntry>> GetAll(CancellationToken cancellationToken);

        Task<LogbookEntry> GetOne(int id, CancellationToken cancellationToken);

        Task<LogbookEntry> Update(LogbookEntry logbookEntry, CancellationToken cancellationToken);

        Task<LogbookEntry> Create(LogbookEntry logbookEntry, CancellationToken cancellationToken);

        Task<LogbookEntry> Delete(int id, CancellationToken cancellationToken);
    }
}
