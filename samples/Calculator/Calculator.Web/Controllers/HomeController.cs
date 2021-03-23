using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Calculator.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Calculator.Web.Models;

namespace Calculator.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICalculationService _calculationService;

        public HomeController(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        public async Task<IActionResult> Index()
        {
            this.ViewData["Result"] = await _calculationService.Calculate(100, 5);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
