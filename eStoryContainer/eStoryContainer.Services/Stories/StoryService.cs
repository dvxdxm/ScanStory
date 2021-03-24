using eStoryContainer.Core.Entities;
using eStoryContainer.Core.Interfaces;
using eStoryContainer.Core.Utils;
using eStoryContainer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eStoryContainer.Services.Stories
{
    public class StoryService : IStoryService
    {
        private readonly IAppLogger<StoryService> _logger;
        protected readonly ApplicationDbContext _dbContext;

        public StoryService(ApplicationDbContext dbContext, IAppLogger<StoryService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Story GetBySlugAsync(string slug)
        {
            return _dbContext.Stories.Where(story => story.slug == slug).FirstOrDefault();
        }

        public async Task<List<Story>> GetListAsync(int pageIndex, int page)
        {
            return await _dbContext.Stories.Where(story => true).Skip((pageIndex + 1) * page).Take(page).OrderByDescending(x => x.modified_on).ToListAsync();
        }

        public async Task<int> CountAllAsync()
        {
            var results = await _dbContext.Stories.Where(story => true).ToListAsync();
            return results.Count;
        }

        public List<Story> SearchByNameAsync(string textSearch, int pageIndex, int page)
        {
            if(textSearch == null)
            {
                _logger.LogWarning("User search with character null");
            }
            return _dbContext.Stories.Where(delegate (Story x) { if (x.story_name.ToUpper().UTF8Convert().Contains(textSearch.ToUpper().UTF8Convert())) { return true; } else { return false; } }).Skip((pageIndex + 1) * page).Take(page).OrderByDescending(s => s.modified_on).ToList();
        }

        public int CountBySearchAsync(string textSearch)
        {
            return _dbContext.Stories.Where(delegate (Story x) { if (x.story_name.ToUpper().UTF8Convert().Contains(textSearch.ToUpper().UTF8Convert())) { return true; } else { return false; } }).Count();
        }
    }
}
