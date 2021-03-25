using System;

namespace eStoryContainer.Core.Common
{
    public class PageResultBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int PageCount
        {
            get
            {
                //var pageCount = (double)TotalRecords / PageSize;
                var psCount = TotalRecords % PageSize;
                //psCount == 0 ? count / page : (count / page) + 1;
                return psCount == 0 ? TotalRecords / PageSize : (TotalRecords / PageSize) + 1; ;
            }
        }
        public string SlugParent { get; set; }
    }
}
