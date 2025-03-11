using System;
using System.ServiceModel.Syndication;
using System.Xml;
using DataAccess;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Models.Models;

namespace Functions
{
    public class GeatherNewsFromRss
    {
        private readonly ILogger _logger;
        private readonly INewsRepository _repository;
        private static readonly Dictionary<string, string> rssUrls = new()
        {
            { "Sky News", "https://feeds.skynews.com/feeds/rss/home.xml" },
            { "New York Times", "https://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml" },
            { "CBS", "https://www.cbsnews.com/latest/rss/main" },
            { "ABC", "https://abcnews.go.com/abcnews/topstories" },
        };

        public GeatherNewsFromRss(ILoggerFactory loggerFactory, INewsRepository repository)
        {
            _logger = loggerFactory.CreateLogger<GeatherNewsFromRss>();
            _repository = repository;
        }

        [Function("GeatherNewsFromRss")]
        public async Task Run([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation("GeatherNewsFromRss started");

            var news = new List<News>();
            foreach (var rss in rssUrls)
            {
                var reader = XmlReader.Create(rss.Value);
                var feed = SyndicationFeed.Load(reader);
                reader.Close();

                foreach (var item in feed.Items)
                {
                    var title = item.Title.Text;

                    if (!await _repository.HasNewsAsync(title))
                    {
                        news.Add(new News
                        {
                            Title = item.Title.Text,
                            Summary = item.Summary.Text,
                            PublishDate = item.PublishDate.DateTime,
                            Source = rss.Key,
                            Authors = item.Authors.Select(a => new Author
                            {
                                Name = a.Name
                            }).ToList(),
                            Links = item.Links.Select(l => new Link
                            {
                                Url = l.Uri.ToString()
                            }).ToList()
                        });
                    }
                }
            }

            foreach (var item in news)
            {
                if (await _repository.HasNewsAsync(item.Title))
                {
                    news.Remove(item);
                }
            }

            await _repository.AddRangeAsync(news);

            _logger.LogInformation($"{news.Count} news were added successfuly");

            _logger.LogInformation("GeatherNewsFromRss ended");
        }
    }
}
