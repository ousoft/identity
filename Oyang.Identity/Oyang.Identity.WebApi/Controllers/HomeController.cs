using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oyang.Identity.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Oyang.Identity.WebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        class Student
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public DateTime CreateTime { get; set; }
        }

        public IActionResult Index()
        {
            var student1 = new Student() { Id = Guid.NewGuid(), Name = "夏欧阳1", Age = 30, CreateTime = DateTime.Now };
            var student2 = new Student() { Id = Guid.NewGuid(), Name = "夏欧阳2", Age = 24, CreateTime = new DateTime(1990, 12, 1) };
            var student3 = new Student() { Id = Guid.NewGuid(), Name = "夏欧阳3", Age = 26, CreateTime = new DateTime(1995, 1, 1) };
            _logger.Log(LogLevel.Debug, "测试Debug：{@student1} / {@student2} / {@student3}", student1, null, student3);
            _logger.Log(LogLevel.Debug, "测试Debug");
            _logger.Log(LogLevel.Information, "测试Information");
            _logger.Log(LogLevel.Warning, "测试Warning");
            _logger.Log(LogLevel.Error, "测试Error");
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
