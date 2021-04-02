using eStoryContainer.Core.Common;
using eStoryContainer.Core.Entities;
using eStoryContainer.Core.Interfaces;
using eStoryContainer.Core.Utils;
using eStoryContainer.Core.ViewModels;
using eStoryContainer.Data;
using eStoryContainer.Services.Stories;
using Microsoft.Extensions.Logging;
using MoreLinq.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace eStoryContainer.Services.Chapters
{
    public class ChapterService : IChapterService
    {
        private readonly ILogger<ChapterService> _logger;
        protected readonly ApplicationDbContext _dbContext;
        protected readonly IStoryService _storyService;

        public ChapterService(ApplicationDbContext dbContext, ILogger<ChapterService> logger, IStoryService storyService )
        {
            _dbContext = dbContext;
            _logger = logger;
            _storyService = storyService;
        }
        public List<Chapter> GetChapters(int pageIndex, int page)
        {
            return _dbContext.Chapters.Where(book => true).Skip((pageIndex + 1) * page).Take(page).OrderByDescending(s => s.modified_on).ToList();
        }

        public List<ChapterViewModel> NewChaptersUpdate(int pageIndex, int page)
        {
            var chapters = _dbContext.Chapters.Where(chapter => true).DistinctBy(d => d.story_name).Skip((pageIndex + 1) * page).Take(page).OrderByDescending(s => s.modified_on);

            foreach (var item in chapters)
            {
                item.Story = _storyService.GetBySlug(item.slug);
            }
            var results = chapters.Select(x => x.Convert()).ToList();
            return results;
        }

        public PageResult<Chapter> GetChapterToStory(string storyName, int pageIndex = 1, int pageSize = 50)
        {
            var totalRecord = _dbContext.Chapters.Where(chapter => chapter.story_name == storyName).ToList().Count;
            var chapters = _dbContext.Chapters.Where(chapter => chapter.story_name == storyName).Skip((pageIndex) * pageSize).Take(pageSize).OrderBy(s => s.sort_number).ToList();
            
            var result = new PageResult<Chapter>()
            {
                TotalRecords = totalRecord,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = chapters
            };
            return result;
        }

        public int CountAllChapters(string storyName)
        {
            return _dbContext.Chapters.Where(chapter => chapter.story_name == storyName).ToList().Count;
        }

        public List<Chapter> GetChapterLastToStory(string storyName, int pageIndex = 0, int page = 5)
        {
            return _dbContext.Chapters.Where(chapter => chapter.story_name == storyName).Take(page).OrderByDescending(s => s.sort_number).ToList();
        }
    }
}
