using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using truyendochay.Models.ViewModel;
using truyendochay.Services;
using Newtonsoft.Json;
using truyendochay.Models;
using System.Collections.Generic;

namespace truyendochay.Controllers
{
    public class StoriesController : Controller
    {
        private readonly ILogger<StoriesController> _logger;
        private readonly ScanStoryService _scanStoryService;
        private readonly int page = 50;
        private readonly int pageIndex = 0;
        private readonly IHostingEnvironment _environment;
        public StoriesController(ILogger<StoriesController> logger, ScanStoryService scanStoryService, IHostingEnvironment environment)
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
            // get list chapter of story
            var listChapters = _scanStoryService.GetChapterToStory(storyName: story.story_name, pageIndex, page);
            // get last chapters 
            var lastChapters = _scanStoryService.GetChapterLastToStory(storyName: story.story_name);
            // count all chapters
            var count = _scanStoryService.CountAllChapters(storyName: story.story_name);
            var psCount = count % page;
            // Tổng số page
            model.PageCount = psCount == 0 ? count / page : (count / page) + 1; 

            model.ListStories = listStories.ConvertAll<CategoryViewModel>(s => s.Convert());
            model.CategoryStories = categories.ConvertAll<CategoryStoryViewModel>(s => s.Convert());
            model.Chapters = listChapters;
            model.LastChapters = lastChapters;
            model.Story = story;
            return View(model);
        }

        [HttpGet("{storyName}/trang-{pageIndex}")]
        public JsonResult NextPage(string storyName, int pageIndex)
        {
            var story = _scanStoryService.GetStory(storyName);
            var datas = _scanStoryService.GetChapterToStory(storyName: story.story_name, pageIndex, page);
            return Json(datas);
        }
    }
}
