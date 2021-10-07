using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nisshi.Infrastructure.Security
{
    public interface IPasswordHasher : IDisposable
    {
        Task<byte[]> HashAsync(string password, byte[] salt, CancellationToken cancellationToken);
        byte[] Hash(string password, byte[] salt);
    }
}