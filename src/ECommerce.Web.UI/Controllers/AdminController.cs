using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.UI.Controllers;

public class AdminController : Controller
{
    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult Inventory()
    {
        return View();
    }

    public IActionResult Products()
    {
        return View();
    }
}
