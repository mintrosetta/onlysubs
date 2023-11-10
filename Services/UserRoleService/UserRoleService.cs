using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlySubs.Context;
using OnlySubs.Models.db;

namespace OnlySubs.Services.UserRoleService
{
    public class UserRoleService : IUserRoleService
    {
        private readonly OnlySubsContext _db;

        public UserRoleService(OnlySubsContext db)
        {
            _db = db;
        }

        public async Task AddRoleByUserIdAsync(string userId, int roleId)
        {
            UsersRole usersRole = new UsersRole 
            { 
                UserId = userId,
                RoleId = roleId
            };
            _db.UsersRoles.Add(usersRole);
            await _db.SaveChangesAsync();
        }

        public async Task<string> GetRoleByUserId(string userId)
        {
            int roleId = await _db.UsersRoles.Where(r => r.UserId == userId).Select(u => u.RoleId).FirstOrDefaultAsync();

            return await _db.Roles.Where(u => u.Id == roleId).Select(r => r.Name).FirstOrDefaultAsync();
        }
    }
}