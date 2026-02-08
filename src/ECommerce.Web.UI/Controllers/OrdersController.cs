using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.UI.Controllers;

public class OrdersController : Controller
{
    public IActionResult History()
    {
        return View();
    }

    public IActionResult Details(Guid id)
    {
        ViewBag.OrderId = id;
        return View();
    }
}
