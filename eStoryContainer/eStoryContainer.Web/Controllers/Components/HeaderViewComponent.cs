using eStoryContainer.Core.Entities;
using eStoryContainer.Core.Interfaces;
using eStoryContainer.Core.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace eStoryContainer.Web.Controllers.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IStoryService _storyService;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryStoryService _categoryStoryService;
        private readonly int page = 21;
        private readonly int pageIndex = 0;
        private readonly IHostingEnvironment _environment;

        public HeaderViewComponent(IStoryService storyService, ICategoryService categoryService, IHostingEnvironment environment, ICategoryStoryService categoryStoryService)
        {
            _storyService = storyService;
            _categoryService = categoryService;
            _categoryStoryService = categoryStoryService;
            _environment = environment;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var root = _environment.WebRootPath;
            var categories = await _categoryStoryService.getCategoriesStory();
            // init thể loại truyện
            if (categories?.Count == 0)
            {
                using (StreamReader r = new StreamReader(Path.Combine(root, "category_name.json")))
                {
                    string json = r.ReadToEnd();
                    var list = JsonConvert.DeserializeObject<List<CategoryStory>>(json);
                    await _categoryStoryService.SetCategoriesStory(list);
                    categories = await _categoryStoryService.getCategoriesStory();
                }
            }
            var listStories = await _categoryService.getCategories();
            // init thể loại truyện
            if (listStories?.Count == 0)
            {
                using (StreamReader r = new StreamReader(Path.Combine(root, "danh-sach-truyen.json")))
                {
                    string json = r.ReadToEnd();
                    var list = JsonConvert.DeserializeObject<List<Category>>(json);
                    await _categoryService.SetCategories(list);
                    listStories = await _categoryService.getCategories();
                }
            }
            var model = new HeaderViewModel()
            {
                ListStories = listStories.ConvertAll<CategoryViewModel>(s => s.Convert()),
                CategoryStories = categories.ConvertAll<CategoryStoryViewModel>(s => s.Convert()),

            };
            return await Task.FromResult((IViewComponentResult)View("_HeaderDefault", model));
        }
    }
}
