using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oyang.Identity.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Oyang.Identity.IApplication.Database;
using Microsoft.AspNetCore.Authorization;

namespace Oyang.Identity.WebApi.ApiControllers
{
    [AllowAnonymous]
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
        public ActionResult<bool> GenerateDatabase()
        {
            return _databaseAppService.GenerateDatabase();
        }

        [HttpGet]
        public void GenerateSeedData()
        {
            _databaseAppService.GenerateSeedData();
        }

    }

}
