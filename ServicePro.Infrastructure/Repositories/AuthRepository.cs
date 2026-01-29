using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServicePro.Core.Entities;
using ServicePro.Core.Interfaces;
using ServicePro.Infrastructure.Data;
using System.Data;

namespace ServicePro.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext context;

        public AuthRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task RegisterUserAsync(User user)
        {
            using IDbConnection db =
                new SqlConnection(context.Database.GetDbConnection().ConnectionString);

            using SqlCommand cmd = new SqlCommand("sp_RegisterUser", (SqlConnection)db);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
            cmd.Parameters.AddWithValue("@Role", user.Role);

            db.Open();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using IDbConnection db =
                new SqlConnection(context.Database.GetDbConnection().ConnectionString);

            using SqlCommand cmd = new SqlCommand("sp_LoginUser", (SqlConnection)db);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);

            db.Open();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!reader.Read()) return null;

            return new User
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                Name = reader["Name"].ToString(),
                Email = reader["Email"].ToString(),
                PasswordHash = reader["PasswordHash"].ToString(),
                Role = reader["Role"].ToString()
            };
        }
    }
}
