import datetime
import json
import os
from pathlib import Path
import scrapy
from ScanStory.Models.Story import Story
from ScanStory.Models.Chapter import Chapter


def download_image_to_link(link):
    # print(f'Scan download image:{link}')
    yield scrapy.Request(link)


def parse_to_link(link):
    # print(f'Current url: {link}')
    request = scrapy.Request(link, callback=get_content_to_url)
    return request


def get_content_of_chapter(link, story_name):
    request = scrapy.Request(link, callback=get_content_chapter, cb_kwargs=dict(story_name=story_name))
    return request


def get_content_chapter(response, story_name):
    item = Chapter()
    content = response.xpath('//div[@id="chapter-c" and not(contains(@class, "ads-network"))]').get()
    print(f"content: {content}")
    chapter_title = response.xpath('//a[contains(@class, "chapter-title")]/@title').get()
    text_replace = story_name + " - "
    text_after_replace = chapter_title.replace(f"{text_replace}", "")

    item['content'] = content
    item["collection_name"] = 'chapter'
    item["chapter_title"] = text_after_replace
    item["story_name"] = story_name
    item['created_by'] = "admin"
    item['created_on'] = datetime.datetime.now()
    item['modified_on'] = datetime.datetime.now()
    item['modified_by'] = "admin"
    yield item


def get_content_to_url(response):
    item = Story()
    avatarPath = response.xpath('//div[contains(@class,"book")]//img/@src').get()
    # if avatarPath != None & avatarPath != '':
    #    yield download_image_to_link(avatarPath)
    story_name = response.xpath('//h3[contains(@class,"title")]/text()').get()
    description = response.xpath('//div[contains(@class, "desc-text")]').get()
    source = response.xpath('//span[contains(@class, "source")]/text()').get()
    status = response.xpath('//span[contains(@class, "text-primary")]/text()').get()
    author = response.xpath('//div[contains(@class, "info")]//a[contains(@itemprop, "author")]/text()').get()
    genre = response.xpath('//div[contains(@class, "info")]//a[contains(@itemprop, "genre")]/text()').getall()
    list_chapter = response.xpath('//ul[contains(@class, "list-chapter")]//a/@title').getall()
    list_urls_chapter = response.xpath('//ul[contains(@class, "list-chapter")]//a/@href').getall()
    # Get content of chapters
    if len(list_urls_chapter) > 0:
        for link_url in list_urls_chapter:
            yield get_content_of_chapter(link_url, story_name)

    after_replace_list_chapter = []
    # Get list name of chapters
    for lc in list_chapter:
        text_replace = story_name + " - "
        after_replace = lc.replace(f"{text_replace}", "")
        after_replace_list_chapter.append(after_replace)

    # item save db
    item['story_name'] = story_name
    item['avatar_path'] = avatarPath
    item['description'] = description
    item['status'] = status
    item['source'] = source
    item['author'] = author
    item['genre'] = genre
    item['created_by'] = "admin"
    item['created_on'] = datetime.datetime.now()
    item['modified_on'] = datetime.datetime.now()
    item['modified_by'] = "admin"
    item["is_deleted"] = 0
    item["hidden"] = 1
    item["list_chapter"] = after_replace_list_chapter
    item["collection_name"] = 'story'

    yield item


class TienHiep(scrapy.Spider):
    name = 'tienhiep'
    allowed_domains = ['truyenfull.vn']
    start_urls = []
    links = []
    FOLDER = Path(__file__).absolute().parent.parent.parent
    my_file = os.path.join(FOLDER, 'assets\TienHiep')

    with open(my_file) as json_file:
        data = json.load(json_file)
        start_urls = data['CategoryUrls']

    def parse(self, response):
        get_links = response.xpath('//h3[@class="truyen-title"]//a/@href').getall()
        for link in get_links:
            self.links.append(link)

        if len(get_links) > 0:
            for link in get_links:
                yield parse_to_link(link)
