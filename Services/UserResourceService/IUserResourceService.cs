using System.Threading.Tasks;
using OnlySubs.Models.db;

namespace OnlySubs.Services.UserResourceService
{
    public interface IUserResourceService
    {
        Task SetResource(int resource, string userId); 
        Task<UsersResource> FindResource(string userId);
        Task<int?> FindMoney(string userId);
        Task AddKrama(int krama, string userId); 
        Task AddMoney(int money, string userId); 
        Task SubtrackKrama(int krama, string userId);
        Task SubtrackMoney(int money, string userId);
        Task Remove(string userId);
    }
}