using Microsoft.AspNetCore.Mvc;

namespace DemoMvc.Controllers;

public class HelloController : Controller
{
    public IActionResult Index()
    {
        return View("Hello");
    }
}
