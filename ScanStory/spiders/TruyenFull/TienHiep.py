import json
import os
from pathlib import Path
import scrapy


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
                yield self.parse_to_link(link)

    def parse_to_link(self, link):
        print(f'Current url: {link}')
        request = scrapy.Request(link, self.get_content_to_url)
        yield request

    def get_content_to_url(self, response):
            title = response.xpath('//h3[contains(@class,"title")]').get()
            print(f'Ten truyen: {title}')



