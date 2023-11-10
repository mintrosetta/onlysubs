using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlySubs.Context;
using OnlySubs.Models.db;

namespace OnlySubs.Services.UserResourceService
{
    public class UserResourceService : IUserResourceService
    {
        private readonly OnlySubsContext _db;

        public UserResourceService(OnlySubsContext db)
        {
            _db = db;
        }

        public async Task SetResource(int resource, string userId)
        {
            UsersResource usersResource = new UsersResource
            {
                UserId = userId,
                Krama = resource,
                Money = resource,
            };
            _db.UsersResources.Add(usersResource);
            await _db.SaveChangesAsync();
        }

        public async Task<UsersResource> FindResource(string userId)
        {
            return await _db.UsersResources.FirstOrDefaultAsync(resource => resource.UserId == userId);
        }

        public async Task<int?> FindMoney(string userId)
        {
            return await _db.UsersResources.Where(r => r.UserId == userId).Select(r => r.Money).FirstOrDefaultAsync();
        }

        public async Task AddKrama(int krama, string userId)
        {
            UsersResource resource = await _db.UsersResources
                                              .FirstOrDefaultAsync(resource => resource.UserId == userId);
            resource.Krama += krama;
            _db.UsersResources.Update(resource);
            await _db.SaveChangesAsync();
        }

        public async Task AddMoney(int money, string userId)
        {
            UsersResource resource = await _db.UsersResources
                                              .FirstOrDefaultAsync(resource => resource.UserId == userId);
            resource.Money += money;
            _db.UsersResources.Update(resource);
            await _db.SaveChangesAsync();
        }

        public async Task SubtrackKrama(int krama, string userId)
        {
            UsersResource resource = await _db.UsersResources
                                              .FirstOrDefaultAsync(resource => resource.UserId == userId);
            resource.Krama -= krama;
            _db.UsersResources.Update(resource);
            await _db.SaveChangesAsync();
        }

        public async Task SubtrackMoney(int money, string userId)
        {
            UsersResource resource = await _db.UsersResources
                                              .FirstOrDefaultAsync(resource => resource.UserId == userId);
            resource.Money -= money;
            _db.UsersResources.Update(resource);
            await _db.SaveChangesAsync();
        }

        public async Task Remove(string userId)
        {
            UsersResource resource = await _db.UsersResources
                                              .FirstOrDefaultAsync(resource => resource.UserId == userId);
            _db.UsersResources.Remove(resource);
            await _db.SaveChangesAsync();
        }
    }
}