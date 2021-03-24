namespace eStoryContainer.Core.Interfaces
{
    public interface IScanStoryDB
    {
        //string BooksCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string DomainUrl { get; set; }
    }
}
