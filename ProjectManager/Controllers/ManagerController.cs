using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectManager.Models;

namespace ProjectManager.Controllers;

[Authorize(Roles="Manager")]
public class ManagerController : Controller
{
    public ManagerController()
    {
    }

    public IActionResult Index()
    {
        TempData["isManager"] = User.IsInRole("Manager");
        TempData["isWorker"] = User.IsInRole("Worker");
        return View();
    }
}

