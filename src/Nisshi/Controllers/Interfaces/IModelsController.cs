using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Models;

namespace Nisshi.Controllers.Interfaces
{
    public interface IModelsController
    {
        Task<IEnumerable<Model>> GetManyByManufacturer(int id, CancellationToken cancellationToken);

        Task<IEnumerable<Model>> GetManyByPartialName(string partialName, CancellationToken cancellationToken);

        Task<Model> GetOneById(int id, CancellationToken cancellationToken);

        Task<Model> Create(Model model, CancellationToken cancellationToken);

        Task<Model> Update(Model model, CancellationToken cancellationToken);
    }
}
