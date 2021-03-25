using eStoryContainer.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eStoryContainer.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> getCategories();
        Task SetCategories(List<Category> categories);
    }
}
