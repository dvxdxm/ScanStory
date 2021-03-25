using eStoryContainer.Core.Entities;
using System.Collections.Generic;

namespace eStoryContainer.Core.ViewModels
{
    public class HeaderViewModel
    {
        public List<CategoryViewModel> ListStories { get; set; }
        public List<CategoryStoryViewModel> CategoryStories { get; set; }
        public Story Story { get; set; }
    }
}
