using eStoryContainer.Core.Entities;
using eStoryContainer.Core.Interfaces;
using eStoryContainer.Core.Utils;
using eStoryContainer.Core.ViewModels;
using eStoryContainer.Data;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace eStoryContainer.Services.Stories
{
    public class StoryService : IStoryService
    {
        private readonly ILogger<StoryService> _logger;
        protected readonly ApplicationDbContext _dbContext;

        public StoryService(ApplicationDbContext dbContext, ILogger<StoryService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Story GetBySlug(string slug)
        {
            return _dbContext.Stories.Where(story => story.slug == slug).FirstOrDefault();
        }

        public List<Story> GetList(int pageIndex, int page)
        {
            return _dbContext.Stories.Where(story => true).Skip((pageIndex + 1) * page).Take(page).OrderByDescending(x => x.modified_on).ToList();
        }

        public int CountAll()
        {
            var results = _dbContext.Stories.Where(story => true).ToList();
            return results.Count;
        }

        public List<Story> SearchByName(string textSearch, int pageIndex, int page)
        {
            if(textSearch == null)
            {
                _logger.LogWarning("User search with character null");
            }
            return _dbContext.Stories.Where(delegate (Story x) { if (x.story_name.ToUpper().UTF8Convert().Contains(textSearch.ToUpper().UTF8Convert())) { return true; } else { return false; } }).Skip((pageIndex + 1) * page).Take(page).OrderByDescending(s => s.modified_on).ToList();
        }

        public int CountBySearch(string textSearch)
        {
            return _dbContext.Stories.Where(delegate (Story x) { if (x.story_name.ToUpper().UTF8Convert().Contains(textSearch.ToUpper().UTF8Convert())) { return true; } else { return false; } }).Count();
        }

        public List<StoryViewModel> SearchTruyenFull(int pageIndex, int page)
        {
            var items = _dbContext.Stories.Where(story => true).Skip((pageIndex + 1) * page).Take(page).OrderByDescending(s => s.modified_on).ToList();

            var totalRecord = _dbContext.Stories.Where(story => true).Count();
            return items.Select(x => x.Convert()).ToList();
        }

        public Story GetByName(string name)
        {
            return _dbContext.Stories.Where(story => story.story_name == name).FirstOrDefault();
        }
    }
}
