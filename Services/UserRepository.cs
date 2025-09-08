using Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Services
{
    public class UserRepository(IConfiguration config) : IUserRepository
    {
        private readonly string? _connString = config.GetConnectionString("DefaultConnection");

        public async Task<User?> GetByEmailAsync(string email)
        {
            await using var conn = new SqlConnection(_connString);
            await conn.OpenAsync();

            var sql = "SELECT TOP(1) * FROM [dbo].[Users] WHERE Email = @Email";

            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    PasswordHash = (byte[])reader["PasswordHash"],
                    PasswordSalt = (byte[])reader["PasswordSalt"],
                    DateCreated = reader.GetDateTime(6)
                };
            }

            return null;
        }

        public async Task<int> CreateAsync(User user)
        {
            await using var conn = new SqlConnection(_connString);
            await conn.OpenAsync();

            var sql = @"
            INSERT INTO [dbo].[Users] (FirstName, LastName, Email, PasswordHash, PasswordSalt)
            OUTPUT INSERTED.Id
            VALUES (@FirstName, @LastName, @Email, @PasswordHash, @PasswordSalt)";

            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);

            return await cmd.ExecuteNonQueryAsync();
        }
    }
}
