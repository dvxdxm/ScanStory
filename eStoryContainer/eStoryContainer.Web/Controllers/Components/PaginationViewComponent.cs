using eStoryContainer.Core.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eStoryContainer.Web.Controllers.Components
{
    public class PaginationViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PageResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
