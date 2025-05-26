using BackEndCyrk.Data;
using BackEndCyrk.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndCyrk.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.email == email);
        }
    }
}