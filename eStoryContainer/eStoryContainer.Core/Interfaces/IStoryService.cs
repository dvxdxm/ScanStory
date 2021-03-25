using eStoryContainer.Core.Entities;
using eStoryContainer.Core.ViewModels;
using System.Collections.Generic;

namespace eStoryContainer.Core.Interfaces
{
    public interface IStoryService
    {
        Story GetBySlug(string slug);
        List<Story> GetList(int pageIndex, int page);
        List<Story> SearchByName(string textSearch, int pageIndex, int page);
        int CountAll();
        int CountBySearch(string textSearch);
        List<StoryViewModel> SearchTruyenFull(int pageIndex, int page);
    }
}
