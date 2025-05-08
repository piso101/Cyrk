using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using BackEndCyrk.Models;

namespace BackEndCyrk.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public async Task<User> RegisterUserAsync(User user, string password)
        {
            if (await UserExistsAsync(user.email))
            {
                throw new Exception("Użytkownik z takim adresem email już istnieje.");
            }
            user.password_hash = HashPassword(password);

            return await AddAsync(user);
        }
        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await GetByEmailAsync(email);

            if (user == null || !VerifyPasswordHash(password, user.password_hash))
            {
                return null;
            }

            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await GetSingleAsync(u => u.email.ToLower() == email.ToLower());
        }

        public async Task<IEnumerable<User>> GetUsersByNameAsync(string name)
        {
            return await GetAsync(u => u.name.Contains(name));
        }

        public async Task<IEnumerable<User>> GetUsersBySurnameAsync(string surname)
        {
            return await GetAsync(u => u.surname.Contains(surname));
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            if (!VerifyPasswordHash(currentPassword, user.password_hash))
            {
                return false;
            }

            user.password_hash = HashPassword(newPassword);

            await UpdateAsync(user);
            return true;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await ExistsAsync(u => u.email.ToLower() == email.ToLower());
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}