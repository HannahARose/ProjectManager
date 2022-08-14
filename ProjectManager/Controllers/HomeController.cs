using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectManager.Models;

namespace ProjectManager.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
	TempData["isManager"] = User.IsInRole("Manager");
	TempData["isWorker"] = User.IsInRole("Worker");
        return View();
    }

    public IActionResult Privacy()
    {
	TempData["isManager"] = User.IsInRole("Manager");
	TempData["isWorker"] = User.IsInRole("Worker");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
	TempData["isManager"] = User.IsInRole("Manager");
	TempData["isWorker"] = User.IsInRole("Worker");
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

