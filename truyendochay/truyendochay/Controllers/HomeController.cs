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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ScanStoryService _scanStoryService;
        private readonly int page = 21;
        private readonly int pageIndex = 0;
        private readonly IHostingEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, ScanStoryService scanStoryService, IHostingEnvironment environment)
        {
            _logger = logger;
            _scanStoryService = scanStoryService;
            _environment = environment;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Client, NoStore = true)]
        public async Task<IActionResult> Index()
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

            model.Chapters = _scanStoryService.GetChapters(pageIndex, page);
            model.Stories = _scanStoryService.GetStories(pageIndex, 13);
            model.CategoryStories = categories.ConvertAll<CategoryStoryViewModel>(s => s.Convert());
            model.ListStories = listStories.ConvertAll<CategoryViewModel>(s => s.Convert());
            model.StoriesFull = _scanStoryService.SearchTruyenFull(pageIndex, 12);
            //truyen mới cập nhật
            model.NewChaptersUpdate = _scanStoryService.NewChaptersUpdate(pageIndex, page);
            return View(model);
        }

        
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
