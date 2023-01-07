using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nisshi.Models.Users;

namespace Nisshi.Controllers.Interfaces
{
    public interface IUsersController
    {
        Task<ActionResult<User>> GetCurrent(CancellationToken cancellationToken);

        Task<ActionResult<User>> UpdateProfile(Profile edit, CancellationToken cancellationToken);

        Task<ActionResult<LoggedIn>> Register(Registration registration, CancellationToken cancellationToken);

        Task<ActionResult<LoggedIn>> Login(Models.Users.Authenticate authenticate, CancellationToken cancellationToken);

        Task<ActionResult<User>> ChangePassword(Models.Users.ChangePasswordModel change, CancellationToken cancellationToken);

        Task<ActionResult<LoggedIn>> RefreshToken(CancellationToken cancellationToken);
    }
}
