using Models;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Services
{
    public class UserRepository(IConfiguration config) : IUserRepository
    {
        private readonly string? _connString = config.GetConnectionString("DefaultConnection");

        private IDbConnection Connection => new SqlConnection(_connString);

        public async Task<User?> GetByEmailAsync(string email)
        {
            const string sql = "SELECT TOP(1) * FROM [dbo].[Users] WHERE Email = @Email";
            using var conn = Connection;
            return await conn.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<int> CreateAsync(User user)
        {
            const string sql = @"
                INSERT INTO [dbo].[Users] (FirstName, LastName, Email, PasswordHash, PasswordSalt)
                OUTPUT INSERTED.Id
                VALUES (@FirstName, @LastName, @Email, @PasswordHash, @PasswordSalt)
            ";
            using var conn = Connection;
            var id = await conn.ExecuteScalarAsync<int>(sql, user);
            return id;
        }
    }
}
