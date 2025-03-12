using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KnorrNews.Models;
using DataAccess;
using Common.Helpers;

namespace KnorrNews.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly INewsRepository _repository; 

    public HomeController(ILogger<HomeController> logger, INewsRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public IActionResult Index()
    {
        var newsList = _repository.GetQueryable().OrderByDescending(n => n.PublishDate).Select(n => new NewsViewModel
        {
            Title = n.Title,
            Summary = n.Summary,
            Source = n.Source,
            PublishDate = n.PublishDate,
            Link = n.Links.FirstOrDefault().Url
        });

        return View(PaginatedList<NewsViewModel>.Create(newsList, 1));
    }

    [HttpGet]
    public IActionResult FilterNews(string title, string source, int page = 1)
    {
        var filteredNews = _repository.GetQueryable().OrderByDescending(n => n.PublishDate).Select(n => new NewsViewModel
        {
            Title = n.Title,
            Summary = n.Summary,
            Source = n.Source,
            PublishDate = n.PublishDate,
            Link = n.Links.FirstOrDefault().Url
        });

        if (!string.IsNullOrEmpty(title))
        {
            filteredNews = filteredNews.Where(n => n.Title.Contains(title));
        }

        if (!string.IsNullOrEmpty(source))
        {
            filteredNews = filteredNews.Where(n => n.Source.Contains(source));
        }

        return PartialView("_NewsList", PaginatedList<NewsViewModel>.Create(filteredNews, page));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
