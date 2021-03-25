using eStoryContainer.Core.Entities;
using eStoryContainer.Core.Interfaces;
using eStoryContainer.Core.ViewModels;
using eStoryContainer.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace eStoryContainer.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStoryService _storyService;
        private readonly IChapterService _chapterService;
        private readonly int page = 21;
        private readonly int pageIndex = 0;
        private readonly IHostingEnvironment _environment;
        private readonly ICategoryStoryService _categoryStoryService;

        public HomeController(ILogger<HomeController> logger, 
            IStoryService storyService, 
            IChapterService chapterService, 
            ICategoryStoryService categoryStoryService, 
            IHostingEnvironment environment)
        {
            _logger = logger;
            _storyService = storyService;
            _chapterService = chapterService;
            _categoryStoryService = categoryStoryService;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
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
            var model = new HomePage() {
                Stories = _storyService.GetList(pageIndex, 13),
                Chapters = _chapterService.GetChapters(pageIndex, page),
                CategoryStories = categories.ConvertAll<CategoryStoryViewModel>(s => s.Convert()),
                NewChaptersUpdate = _chapterService.NewChaptersUpdate(pageIndex, page),
                StoriesFull = _storyService.SearchTruyenFull(pageIndex, page)
            };
            return View(model);
        }

        [HttpGet("tim-kiem")]
        public JsonResult Search(string textSearch = null)
        {
            var datas = _storyService.SearchByName(textSearch, pageIndex, 5);
            return Json(datas);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
