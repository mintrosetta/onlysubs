using System.Collections.Generic;
using System.Threading.Tasks;
using OnlySubs.ViewModels.Responses;

namespace OnlySubs.Services.SearchService
{
    public interface ISearchService
    {
         Task<List<SearchResponse>> Finds(string search);
    }
}