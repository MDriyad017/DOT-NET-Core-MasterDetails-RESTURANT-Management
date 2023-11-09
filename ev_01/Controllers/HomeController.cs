using ev_01.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ev_01.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }

        
    }
}