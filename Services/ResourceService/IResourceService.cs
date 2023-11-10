namespace OnlySubs.Services.ResourceService
{
    public interface IResourceService
    {
        bool ValidateCode(string code);
        int FindResource(string code);
    }
}