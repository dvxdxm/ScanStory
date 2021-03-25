using MongoDB.Bson;
using System;

namespace eStoryContainer.Core.ViewModels
{
    public class StoryViewModel
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
    }
}
