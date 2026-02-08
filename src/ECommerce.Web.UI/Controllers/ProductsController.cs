using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.UI.Controllers;

public class ProductsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Details(Guid id)
    {
        ViewBag.ProductId = id;
        return View();
    }
}
