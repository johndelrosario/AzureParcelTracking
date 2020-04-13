using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AzureParcelTracking.Application.Exceptions;
using AzureParcelTracking.Application.Models;
using AzureParcelTracking.Application.Repositories.Interfaces;

namespace AzureParcelTracking.Application.Repositories.Implementation
{
    internal class UserRepository : BaseRepository<UserRecord>, IUserRepository
    {
        private const int SaltSize = 64;
        private const int DerivedBytesSize = 256;
        private const int DerivedBytesIteration = 10000;
        private const string InvalidCredentialsMessage = "Invalid Credentials";

        static UserRepository()
        {
            Collection.Add(GenerateUser("admin", "password"));
            Collection.Add(GenerateUser("admin2", "password2"));
            Collection.Add(GenerateUser("admin3", "password3"));
        }

        public override Task<Guid> Add(UserRecord item, Guid userId)
        {
            throw new InvalidOperationException("Please use overload with username and password");
        }

        public Task<Guid> Add(string username, string password)
        {
            var generatedUser = GenerateUser(username, password);
            Collection.Add(generatedUser);

            return Task.FromResult(generatedUser.Id);
        }

        public Task<UserRecord> GetByCredentials(string username, string password)
        {
            var targetUser = Collection.Single(record => record.Username == username);

            var saltBytes = Convert.FromBase64String(targetUser.Salt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, DerivedBytesIteration);

            if (Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(DerivedBytesSize)) == targetUser.Password)
                return Task.FromResult(targetUser);

            throw new InvalidCredentialsException(InvalidCredentialsMessage);
        }

        private static UserRecord GenerateUser(string username, string password)
        {
            var saltBytes = new byte[SaltSize];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(saltBytes);

            var salt = Convert.ToBase64String(saltBytes);

            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, DerivedBytesIteration);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(DerivedBytesSize));

            var userRecord = new UserRecord
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = hashPassword,
                Salt = salt,
                CreatedAtUtc = DateTime.UtcNow
            };

            return userRecord;
        }
    }
}