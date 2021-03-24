using eStoryContainer.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eStoryContainer.Core.Interfaces
{
    public interface IStoryService
    {
        Story GetBySlugAsync(string slug);
        Task<List<Story>> GetListAsync(int pageIndex, int page);
        List<Story> SearchByNameAsync(string textSearch, int pageIndex, int page);
        Task<int> CountAllAsync();
        int CountBySearchAsync(string textSearch);
    }
}
