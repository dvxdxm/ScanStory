using eStoryContainer.Core.Entities;
using eStoryContainer.Core.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace eStoryContainer.Data
{
    public class ApplicationDbContext
    {
        private readonly IMongoDatabase _database = null;
        public ApplicationDbContext(IScanStoryDB settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            if (client != null) _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<Category> Categorires
        {
            get
            {
                return _database.GetCollection<Category>("category");
            }
        }
        public IMongoCollection<CategoryStory> CategoryStory
        {
            get
            {
                return _database.GetCollection<CategoryStory>("category_story");
            }
        }
        public IMongoQueryable<Story> Stories
        {
            get
            {
                return _database.GetCollection<Story>("story").AsQueryable();
            }
        }
        public IMongoQueryable<Chapter> Chapters
        {
            get
            {
                return _database.GetCollection<Chapter>("chapter").AsQueryable();
            }
        }
        //public DbSet<Category> Categorires { get; set; }
        //public DbSet<CategoryStory> CategoryStory { get; set; }
        //public DbSet<Chapter> Chapter { get; set; }
        //public DbSet<Story> Story { get; set; }
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //}
    }
}
