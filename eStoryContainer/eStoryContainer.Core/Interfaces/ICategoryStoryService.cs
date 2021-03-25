using eStoryContainer.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eStoryContainer.Core.Interfaces
{
    public interface ICategoryStoryService
    {
       Task SetCategoriesStory(List<CategoryStory> categories);
       Task<List<CategoryStory>> getCategoriesStory();
    }
}
