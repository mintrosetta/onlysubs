using System.Linq;
using OnlySubs.Context;

namespace OnlySubs.Services.ResourceService
{
    public class ResourceService : IResourceService
    {
        private readonly OnlySubsContext _db;

        public ResourceService(OnlySubsContext db)
        {
            _db = db;
        }

        public bool ValidateCode(string code)
        {
            return _db.CodeResources.Where(c => c.Id == code).FirstOrDefault() != null;
        }
        public int FindResource(string code)
        {
            return _db.CodeResources.Where(c => c.Id == code).Select(c => c.Resource).FirstOrDefault();
        }
    }
}