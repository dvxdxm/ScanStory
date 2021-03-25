using eStoryContainer.Core.ViewModels;
using MongoDB.Bson;
using System;

namespace eStoryContainer.Core.Entities
{
    public class Category
    {
        public ObjectId _id { get; set; }
        public string category_name { get; set; }
        public string created_by { get; set; }
        public DateTime created_on { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_on { get; set; }
        public int hidden { get; set; }
        public int is_deleted { get; set; }
        public CategoryViewModel Convert()
        {
            return new CategoryViewModel
            {
                _id = _id,
                category_name = category_name,
                created_by = created_by,
                created_on = created_on,
                modified_by = modified_by,
                modified_on = modified_on,
                hidden = hidden,
                is_deleted = is_deleted,
                slug = category_name
            };
        }
    }
}
