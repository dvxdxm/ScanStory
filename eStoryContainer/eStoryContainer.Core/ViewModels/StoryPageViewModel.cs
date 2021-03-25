using eStoryContainer.Core.Common;
using eStoryContainer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace eStoryContainer.Core.ViewModels
{
    public class StoryPageViewModel
    {
        public Story Story { get; set; }
        public List<Chapter> LastChapters { get; set; }
        //public List<Chapter> Chapters { get; set; }
        public PageResultBase Pagination { get; set; }
        public PageResult<Chapter> Chapters { get; set; }
    }
}
