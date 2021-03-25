using eStoryContainer.Core.Entities;
using eStoryContainer.Core.Interfaces;
using eStoryContainer.Data;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace eStoryContainer.Services.Categories
{
    public class CategoryStoryService: ICategoryStoryService
    {
        private readonly ILogger<CategoryStoryService> _logger;
        protected readonly ApplicationDbContext _dbContext;

        public CategoryStoryService(ApplicationDbContext dbContext, ILogger<CategoryStoryService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task SetCategoriesStory(List<CategoryStory> categories) => await _dbContext.CategoryStory.InsertManyAsync(categories);

        public async Task<List<CategoryStory>> getCategoriesStory()
        {
            return await _dbContext.CategoryStory.FindSync(cate => true).ToListAsync();
        }
    }
}
