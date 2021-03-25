using eStoryContainer.Core.Entities;
using eStoryContainer.Core.Interfaces;
using eStoryContainer.Core.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace eStoryContainer.Web.Controllers
{
    public class StoriesController : Controller
    {
        private readonly ILogger<StoriesController> _logger;
        private readonly int page = 50;
        private readonly int pageIndex = 1;
        private readonly IHostingEnvironment _environment;
        private readonly IStoryService _storyService;
        private readonly IChapterService _chapterService;
        private readonly ICategoryStoryService _categoryStoryService;
        private readonly ICategoryService _categoryService;
        public StoriesController(ILogger<StoriesController> logger,
            IStoryService storyService,
            IChapterService chapterService,
            ICategoryStoryService categoryStoryService,
            IHostingEnvironment environment,
            ICategoryService categoryService)
        {
            _logger = logger;
            _environment = environment;
            _storyService = storyService;
            _chapterService = chapterService;
            _categoryStoryService = categoryStoryService;
            _categoryService = categoryService;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Client, NoStore = true)]
        public async Task<IActionResult> Index([FromRoute] string slug)
        {
            var model = new StoryPageViewModel();
            var categories = await _categoryStoryService.getCategoriesStory();
            var root = _environment.WebRootPath;
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
            
            // init danh sách truyện
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
            //get story
            var story = _storyService.GetBySlug(slug);
            // get list chapter of story
            var listChapters = _chapterService.GetChapterToStory(storyName: story.story_name, pageIndex, page);
            // get last chapters 
            var lastChapters = _chapterService.GetChapterLastToStory(storyName: story.story_name);
            listChapters.SlugParent = story.slug;

            model.Chapters = listChapters;
            model.LastChapters = lastChapters;
            model.Story = story;
            return View(model);
        }

        [HttpGet("{slug}/trang-{index}")]
        public async Task<IActionResult> GetChapters(string slug, int index)
        {
            var model = new StoryPageViewModel();
            var categories = await _categoryStoryService.getCategoriesStory();
            var root = _environment.WebRootPath;
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

            // init danh sách truyện
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
            //get story
            var story = _storyService.GetBySlug(slug);
            // get list chapter of story
            var listChapters = _chapterService.GetChapterToStory(storyName: story.story_name, index, page);
            // get last chapters 
            var lastChapters = _chapterService.GetChapterLastToStory(storyName: story.story_name);
            listChapters.SlugParent = story.slug;

            model.Chapters = listChapters;
            model.LastChapters = lastChapters;
            model.Story = story;
            return View("Index", model);
        }
    }
}
