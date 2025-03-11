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
        private readonly INewsRepository repository;
        private static readonly string[] rssUrls =
        [
            "https://feeds.skynews.com/feeds/rss/home.xml",
            "https://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml",
            "https://www.cbsnews.com/latest/rss/main",
            "https://abcnews.go.com/abcnews/topstories",
        ];

        public GeatherNewsFromRss(ILoggerFactory loggerFactory, INewsRepository repository)
        {
            _logger = loggerFactory.CreateLogger<GeatherNewsFromRss>();
            this.repository = repository;
        }

        [Function("GeatherNewsFromRss")]
        public async Task Run([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var a = await repository.GetListAsync();

            var news = new List<News>();
            foreach (var url in rssUrls)
            {
                var reader = XmlReader.Create(url);
                var feed = SyndicationFeed.Load(reader);
                reader.Close();

                foreach (var item in feed.Items)
                {
                    var title = item.Title.Text;

                    if (!await repository.HasNewsAsync(title))
                    {
                        news.Add(new News
                        {
                            Title = item.Title.Text,
                            Summary = item.Summary.Text,
                            PublishDate = item.PublishDate.DateTime,
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
                if (await repository.HasNewsAsync(item.Title))
                {
                    news.Remove(item);
                }
            }

            await repository.AddRangeAsync(news);

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
