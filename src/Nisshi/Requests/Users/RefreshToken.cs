using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Infrastructure.Security;
using Nisshi.Models.Users;

namespace Nisshi.Requests.Users
{
    public class RefreshToken
    {
        public record Command() : IRequest<LoggedIn>;

        public class CommandHandler : BaseHandler, IRequestHandler<Command, LoggedIn>
        {
            private readonly ICurrentUserAccessor accessor;
            private readonly IPasswordHasher hasher;
            private readonly IJwtTokenGenerator bigGenerator;
            private readonly IMapper mapper;

            public CommandHandler(NisshiContext context,
                                ICurrentUserAccessor accessor,
                                IPasswordHasher hasher,
                                IJwtTokenGenerator bigGenerator,
                                IMapper mapper) : base(context)
            {
                this.accessor = accessor;
                this.hasher = hasher;
                this.bigGenerator = bigGenerator;
                this.mapper = mapper;
            }

            public async Task<LoggedIn> Handle(Command request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var user = await base.context.Users
                    .SingleOrDefaultAsync(x => x.Username == username, cancellationToken);

                if (user == null)
                    throw new DomainException(typeof(User), Message.ItemDoesNotExist);

                // Maps to a model that includes a JWT token for Angular
                var loggedInUser = mapper.Map<User, LoggedIn>(user);
                loggedInUser.Token = bigGenerator
                    .CreateToken(user.Username ?? throw new InvalidOperationException(), user.UserType.ToString());

                return loggedInUser;
            }
        }
    }
}
