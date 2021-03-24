using eStoryContainer.Core.Entities;
using System.Collections.Generic;

namespace eStoryContainer.ViewModels
{
    public class HomePage
    {
        public List<Chapter> Chapters { get; set; }
        public List<Story> Stories { get; set; }
        public List<CategoryStoryViewModel> CategoryStories { get; set; }
        public List<CategoryViewModel> ListStories { get; set; }
        public List<ChapterViewModel> NewChaptersUpdate { get; set; }
        public List<StoryViewModel> StoriesFull { get; set; }
        public List<Chapter> LastChapters { get; set; }
        public int? CountChapters { get;set; }
        public int? PageCount { get;set; }
        public Story Story { get; set; }
    }
}
