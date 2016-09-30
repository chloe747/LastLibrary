using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LastLibrary.Controllers
{
    public class DeckBuildingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}