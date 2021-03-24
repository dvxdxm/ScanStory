using eStoryContainer.Services.Stories;
using eStoryContainer.ViewModels;
using eStoryContainer.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using truyendochay.Models.ViewModel;

namespace eStoryContainer.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StoryService _storyService;
        private readonly int page = 21;
        private readonly int pageIndex = 0;
        private readonly IHostingEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, StoryService storyService)
        {
            _logger = logger;
            _storyService = storyService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomePage() {
                Stories = await _storyService.GetListAsync(pageIndex, 13)
            };
            return View(model);
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
