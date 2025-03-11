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

    public IActionResult Index()
    {
        var a = _repository.GetListAsync().Result;
        return View();
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
