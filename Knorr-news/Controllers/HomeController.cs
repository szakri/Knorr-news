using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KnorrNews.Models;
using DataAccess;

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

    public async Task<IActionResult> Index()
    {
        var news = (await _repository.GetListAsync()).Select(n => new NewsViewModel
        {
            Id = n.Id,
            Title = n.Title,
            Summary = n.Summary,
            Source = n.Source,
            PublishDate = n.PublishDate,
            Link = n.Links.FirstOrDefault()?.Url ?? string.Empty
        }).ToList();

        return View(news);
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
