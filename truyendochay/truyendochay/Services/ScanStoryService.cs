using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using truyendochay.Models;
using truyendochay.Models.Interfaces;
using truyendochay.Models.ViewModel;
using truyendochay.Utils;

namespace truyendochay.Services
{

    public class ScanStoryService
    {
        private readonly IMongoQueryable<Chapter> _chapter;
        private readonly IMongoQueryable<Story> _story;
        private readonly IMongoCollection<CategoryStory> _cateStory;
        private readonly IMongoCollection<Category> _cate;
        public ScanStoryService(IScanStoryDB settings)
        {
            //_client = new MongoClient("mongodb://localhost:27017");
            //_server = _client.GetServer();
            //_db = _server.GetDatabase("BookstoreDb");
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _chapter = database.GetCollection<Chapter>("chapter").AsQueryable();
            _story = database.GetCollection<Story>("story").AsQueryable();
            _cateStory = database.GetCollection<CategoryStory>("category_story");
            _cate = database.GetCollection<Category>("category");
        }
        #region Chapters
        public List<Chapter> GetChapters(int pageIndex, int page) => _chapter.Where(book => true).Skip((pageIndex + 1) * page).Take(page).OrderByDescending (s => s.modified_on).ToList();
        public  List<ChapterViewModel> NewChaptersUpdate(int pageIndex, int page)
        {
            var chapters = _chapter.Where(chapter => true).Skip((pageIndex + 1) * page).Take(page).OrderByDescending(s => s.modified_on).ToList();
            foreach(var item in chapters)
            {
                item.Story = GetStory(item.slug);
            }
            //var newChapters = chapters.SelectMany(x => x.Story = GetStory(x.story_name)).ToList();
            var results = chapters.Select(x => x.Convert()).ToList();
            return results;
        }
        public List<Chapter> GetChapterToStory(string storyName, int pageIndex = 0, int page = 50)
        {
            var count = _chapter.Where(chapter => chapter.story_name == storyName).ToList().Count;
            var chapters = _chapter.Where(chapter => chapter.story_name == storyName).Skip((pageIndex + 1) * page).Take(page).OrderBy(s => s.sort_number).ToList();
            return chapters;
        }
        public int CountAllChapters(string storyName)
        {
            var count = _chapter.Where(chapter => chapter.story_name == storyName).ToList().Count;
            return count;
        }

        public List<Chapter> GetChapterLastToStory(string storyName, int pageIndex = 0, int page = 5)
        {
            var chapters = _chapter.Where(chapter => chapter.story_name == storyName).Take(page).OrderByDescending(s => s.sort_number).ToList();
            return chapters;
        }
        #endregion
        #region Stories
        public Story GetStory(string name) => _story.Where(story => story.slug == name).FirstOrDefault();
        public List<Story> GetStories(int pageIndex, int page) => _story.Where(story => true).Skip((pageIndex + 1) * page).Take(page).OrderByDescending(x => x.modified_on).ToList();
        public List<Story> Search(string textSearch, int pageIndex, int page)
        {
            var items = _story.Where(delegate (Story x) { if (x.story_name.ToUpper().UTF8Convert().Contains(textSearch.ToUpper().UTF8Convert())) { return true; } else { return false; } }).Skip((pageIndex + 1) * page).Take(page).OrderByDescending(s => s.modified_on).ToList();

            var totalRecord = _story.Where(delegate (Story x) { if (x.story_name.ToUpper().UTF8Convert().Contains(textSearch.ToUpper().UTF8Convert())) { return true; } else { return false; } }).Count();
            return items;
        }
        public List<StoryViewModel> SearchTruyenFull(int pageIndex, int page)
        {
            var items = _story.Where(story => true).Skip((pageIndex + 1) * page).Take(page).OrderByDescending(s => s.modified_on).ToList();

            var totalRecord = _story.Where(story => true).Count();
            return items.Select(x => x.Convert()).ToList();
        }
        #endregion
        #region
        public async Task SetCategoriesStory(List<CategoryStory> categories) => await _cateStory.InsertManyAsync(categories);
        public async Task<List<CategoryStory>> getCategoriesStory() => await _cateStory.FindAsync(cate => true).Result.ToListAsync();
        #endregion
     
        
        #region Init Danh sach truyện
        public async Task<List<Category>> getCategories() => await _cate.FindAsync(cate => true).Result.ToListAsync();
        public async Task SetCategories(List<Category> categories) => await _cate.InsertManyAsync(categories);
        #endregion
    }
}
