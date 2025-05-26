using BackEndCyrk.Models;

namespace BackEndCyrk.Repositories
{
    public interface IUserRepository : IBaseRepository.IBaseRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}