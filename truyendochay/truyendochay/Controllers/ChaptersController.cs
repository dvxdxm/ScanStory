using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using truyendochay.Models;
using truyendochay.Models.ViewModel;
using truyendochay.Services;

namespace truyendochay.Controllers
{
    public class ChaptersController : Controller
    {
        private readonly ILogger<ChaptersController> _logger;
        private readonly ScanStoryService _scanStoryService;
        private readonly int page = 50;
        private readonly int pageIndex = 0;
        private readonly IHostingEnvironment _environment;
        public ChaptersController(ILogger<ChaptersController> logger, ScanStoryService scanStoryService, IHostingEnvironment environment)
        {
            _logger = logger;
            _scanStoryService = scanStoryService;
            _environment = environment;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Client, NoStore = true)]
        public async Task<IActionResult> Index([FromRoute] string slug)
        {
            var model = new HomePage();
            var categories = await _scanStoryService.getCategoriesStory();
            var root = _environment.WebRootPath;
            // init thể loại truyện
            if (categories?.Count == 0)
            {
                using (StreamReader r = new StreamReader(Path.Combine(root, "category_name.json")))
                {
                    string json = r.ReadToEnd();
                    var list = JsonConvert.DeserializeObject<List<CategoryStory>>(json);
                    await _scanStoryService.SetCategoriesStory(list);
                    categories = await _scanStoryService.getCategoriesStory();
                }
            }
            // init danh sách truyện
            var listStories = await _scanStoryService.getCategories();
            // init thể loại truyện
            if (listStories?.Count == 0)
            {
                using (StreamReader r = new StreamReader(Path.Combine(root, "danh-sach-truyen.json")))
                {
                    string json = r.ReadToEnd();
                    var list = JsonConvert.DeserializeObject<List<Category>>(json);
                    await _scanStoryService.SetCategories(list);
                    listStories = await _scanStoryService.getCategories();
                }
            }
            //get story
            var story = _scanStoryService.GetStory(slug);
           

            model.ListStories = listStories.ConvertAll<CategoryViewModel>(s => s.Convert());
            model.CategoryStories = categories.ConvertAll<CategoryStoryViewModel>(s => s.Convert());
            
            model.Story = story;
            return View(model);
        }
    }
}
