using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace eStoryContainer.ViewModels
{
    public class ChapterViewModel
    {
        public ObjectId _id { get; set; }
        public string chapter_title { get; set; }
        public string collection_name { get; set; }
        public string content { get; set; }
        public string created_by { get; set; }
        public DateTime created_on { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_on { get; set; }
        public int sort_number { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string story_id { get; set; }
        public string story_name { get; set; }
        public string slug { get; set; }
        public StoryViewModel Story { get; set; } 
    }
}
