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


def request_get_list_chapters(link):
    res = yield scrapy.Request(link, callback=response_list_chapters)
    return res


def response_list_chapters(response):
    list_urls_chapter = response.xpath('//ul[contains(@class, "list-chapter")]//a/@href').getall()

    return list_urls_chapter


def request_get_content_story(link):
    # print(f'Current url: {link}')
    request = scrapy.Request(link, callback=get_content_story_to_url, cb_kwargs=dict(link=link))
    return request


def request_get_content_of_chapter(link, story_name):
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


def get_content_story_to_url(response, link):
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
    last_page_text = response.xpath('//ul[contains(@class, "pagination")]//li[not(contains(@class,"active"))]//a[text('
                                    ')="Cuối "]/@title').get()

    if last_page_text:
        text_replace_page = story_name + " - Trang "
        replace_last_page_text = last_page_text.replace(text_replace_page, "")
        if int(replace_last_page_text) > 0:
            for index in range(int(replace_last_page_text)):
                link_to_page = link + "trang-" + str(index+1)
                print(f"link_to_page: {link_to_page}")
                # TODO: get list chapters
                new_list_chapters = request_get_list_chapters(link_to_page)
                print(f"new_list_chapters: {new_list_chapters}")

    else:
        list_pages = response.xpath('//ul[contains(@class, "pagination")]//li[not(contains(@class,"active"))]//a[not('
                                    'span)]/text()').getall()
        if len(list_pages) > 0:
            number_index_pages = list_pages[len(list_pages) - 1]
            for index in range(int(number_index_pages)):
                print(f"index: {index}")
                # TODO: get list chapters

    # Get content of chapters
    # TODO: get list chapter theo trang
    # if len(list_urls_chapter) > 0:
    #    for link_url in list_urls_chapter:
    #        yield request_get_content_of_chapter(link_url, story_name)

    after_replace_list_chapter = []
    # Get list name of chapters
    # TODO: get list chapter theo trang
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
                yield request_get_content_story(link)
