using eStoryContainer.Core.Entities;
using eStoryContainer.Core.Interfaces;
using eStoryContainer.Data;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eStoryContainer.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        protected readonly ApplicationDbContext _dbContext;

        public CategoryService(ApplicationDbContext dbContext, ILogger<CategoryService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<Category>> getCategories()
        {
            return await _dbContext.Categorires.FindSync(cate => true).ToListAsync();
        }

        public async Task SetCategories(List<Category> categories) => await _dbContext.Categorires.InsertManyAsync(categories);
    }
}
