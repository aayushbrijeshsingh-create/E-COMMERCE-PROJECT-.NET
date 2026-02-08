using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.UI.Controllers;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Profile()
    {
        return View();
    }

    public IActionResult Logout()
    {
        return RedirectToAction("Index", "Home");
    }
}
