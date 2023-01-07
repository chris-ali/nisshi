using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Models;

namespace Nisshi.Controllers.Interfaces
{
    public interface ICategoryClassesController
    {
        Task<IEnumerable<CategoryClass>> GetAll(CancellationToken cancellationToken);

        Task<CategoryClass> GetOneById(int id, CancellationToken cancellationToken);
    }
}
