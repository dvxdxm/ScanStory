import scrapy


class Story(scrapy.Item):
    story_id = scrapy.Field()
    story_name = scrapy.Field()
    slug = scrapy.Field()
    description = scrapy.Field()
    cost = scrapy.Field()
    status = scrapy.Field()
    unassigned_coin = scrapy.Field()
    origin_story_id = scrapy.Field()
    translated_from_story_id = scrapy.Field()
    feature_image = scrapy.Field()
    language_id = scrapy.Field()
    translator_id = scrapy.Field()
    published_date = scrapy.Field()
    created_on = scrapy.Field()
    created_by = scrapy.Field()
    modified_on = scrapy.Field()
    modified_by = scrapy.Field()
    is_deleted = scrapy.Field()
    hidden = scrapy.Field()
