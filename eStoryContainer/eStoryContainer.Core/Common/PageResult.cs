using System.Collections.Generic;

namespace eStoryContainer.Core.Common
{
    public class PageResult<T>: PageResultBase
    {
        public List<T> Items { get; set; }
    }
}
