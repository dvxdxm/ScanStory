using eStoryContainer.Core.Common;
using eStoryContainer.Core.Entities;
using eStoryContainer.Core.ViewModels;
using System.Collections.Generic;

namespace eStoryContainer.Core.Interfaces
{
    public interface IChapterService
    {
        List<Chapter> GetChapters(int pageIndex, int page);
        List<ChapterViewModel> NewChaptersUpdate(string catName, int pageIndex, int page);
        PageResult<Chapter> GetChapterToStory(string storyName, int pageIndex = 0, int page = 50);
        int CountAllChapters(string storyName);
        List<Chapter> GetChapterLastToStory(string storyName, int pageIndex = 0, int page = 5);
    }
}
