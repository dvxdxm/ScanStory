using MongoDB.Bson;
using System;
using truyendochay.Models.ViewModel;

namespace truyendochay.Models
{
    public class Story
    {
        public ObjectId _id { get; set; }
        public string author { get; set; }
        public string avatar_path { get; set; }
        public string collection_name { get; set; }
        public string created_by { get; set; }
        public DateTime created_on { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_on { get; set; }
        public string description { get; set; }
        public string description_seo { get; set; }
        public string[] genre { get; set; }
        public int hidden { get; set; }
        public int is_deleted { get; set; }
        public string keywords { get; set; }
        public string source { get; set; }
        public string status { get; set; }
        public string story_name { get; set; }
        public string slug { get; set; }
        public StoryViewModel Convert()
        {
            if (_id.Equals(null)) return null;
            return new StoryViewModel
            {
                _id = _id,
                story_name = story_name,
                created_by = created_by,
                created_on = created_on,
                modified_by = modified_by,
                modified_on = modified_on,
                collection_name = collection_name,
                status = status,
                source = source,
                keywords = keywords,
                is_deleted = is_deleted,
                hidden = hidden,
                genre = genre,
                description_seo = description_seo,
                description = description,
                avatar_path = avatar_path,
                author = author,
                slug = !string.IsNullOrWhiteSpace(slug) ? string.Format("/{0}", slug) : null
            };
        }
    }
}
