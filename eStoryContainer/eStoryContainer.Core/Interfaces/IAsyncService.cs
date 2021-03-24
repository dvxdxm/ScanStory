using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace eStoryContainer.Core.Interfaces
{
    public interface IAsyncService<T> where T: IAggregateRoot
    {
        Task<T> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<List<T>> GetListAsync(int pageIndex, int page);
        Task<List<T>> SearchByNameAsync(string textSearch, int pageIndex, int page);
        Task<int> CountAllAsync(string storyName);
        //Task SetCategoriesStoryAsync(List<T> list);
    }
}
