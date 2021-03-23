using MongoDB.Bson;
using System;

namespace truyendochay.Models.ViewModel
{
    public class CategoryStoryViewModel
    {
        public ObjectId _id { get; set; }
        public string category_story_name { get; set; }
        public string created_by { get; set; }
        public DateTime created_on { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_on { get; set; }
        public int hidden { get; set; }
        public int is_deleted { get; set; }
        public string? slug { get; set; }
    }
}
