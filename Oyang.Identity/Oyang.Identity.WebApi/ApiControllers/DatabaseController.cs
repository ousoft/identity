using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oyang.Identity.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Oyang.Identity.IApplication.Database;

namespace Oyang.Identity.WebApi.ApiControllers
{
    public class DatabaseController : BaseApiController
    {
        private readonly ILogger<DatabaseController> _logger;
        private readonly IDatabaseAppService _databaseAppService;

        public DatabaseController(
            ILogger<DatabaseController> logger,
            IDatabaseAppService databaseAppService
            )
        {
            _logger = logger;
            _databaseAppService = databaseAppService;
        }

        [HttpGet]
        public IActionResult GenerateDatabase()
        {
            var model = _databaseAppService.GenerateDatabase();
            return Ok(model);
        }

        [HttpGet]
        public IActionResult GenerateSeedData()
        {
            _databaseAppService.GenerateSeedData();
            return Ok();
        }

    }

}
