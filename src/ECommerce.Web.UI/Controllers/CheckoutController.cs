using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.UI.Controllers;

public class CheckoutController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
