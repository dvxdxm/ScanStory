using eStoryContainer.Core.ViewModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace eStoryContainer.Core.Entities
{
    public class Chapter
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
        public Story Story { get; set; }
        public string slug { get; set; }
        public ChapterViewModel Convert()
        {

            return new ChapterViewModel
            {
                _id = _id,
                chapter_title = chapter_title,
                created_by = created_by,
                created_on = created_on,
                modified_by = modified_by,
                modified_on = modified_on,
                collection_name = collection_name,
                story_id = story_id,
                story_name = story_name,
                slug = slug,
                Story = Story == null ? null : Story.Convert()
            };
        }
    }
}
