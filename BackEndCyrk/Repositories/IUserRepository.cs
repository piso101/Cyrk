using System.Threading.Tasks;
using System.Collections.Generic;
using BackEndCyrk.Models;

namespace BackEndCyrk.Repositories
{
    public interface IUserRepository : IBaseRepository.IBaseRepository<User>
    {
        Task<User> RegisterUserAsync(User user, string password);
        Task<User> AuthenticateAsync(string email, string password);
        Task<User> GetByEmailAsync(string email);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> UserExistsAsync(string email);
        Task<IEnumerable<User>> GetUsersByNameAsync(string name);
        Task<IEnumerable<User>> GetUsersBySurnameAsync(string surname);
    }
}