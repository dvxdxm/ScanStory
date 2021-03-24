using eStoryContainer.Core.Interfaces;

namespace eStoryContainer.Models
{
    public class ScanStoryDB: IScanStoryDB
    {
        //public string BooksCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string DomainUrl { get; set; }
    }
}
