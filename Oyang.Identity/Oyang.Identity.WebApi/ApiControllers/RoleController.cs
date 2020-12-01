using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oyang.Identity.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Oyang.Identity.IApplication.Role;
using Oyang.Identity.IApplication.Role.Dtos;
using Oyang.Identity.Infrastructure.Common;

namespace Oyang.Identity.WebApi.ApiControllers
{
    public class RoleController : BaseApiController
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleAppService  _roleAppService;

        public RoleController(
            ILogger<RoleController> logger,
            IRoleAppService roleAppService
            )
        {
            _logger = logger;
            _roleAppService = roleAppService;
        }

        [HttpGet]
        [Permission(nameof(PermissionNames.Role_GetList))]
        public IActionResult GetList(GetListInputDto input)
        {
            var model = _roleAppService.GetList(input);
            return Ok(model);
        }

        [HttpGet("id")]
        [Permission(nameof(PermissionNames.Role_Get))]
        public IActionResult Get(Guid id)
        {
            var model = _roleAppService.Get(id);
            return Ok(model);
        }

        [HttpPost]
        [Permission(nameof(PermissionNames.Role_Add))]
        public IActionResult Add(AddInputDto input)
        {
            _roleAppService.Add(input);
            return Ok();
        }

        [HttpPost]
        [Permission(nameof(PermissionNames.Role_Update))]
        public IActionResult Update(UpdateInputDto input)
        {
            _roleAppService.Update(input);
            return Ok();
        }

        [HttpPost("id")]
        [Permission(nameof(PermissionNames.Role_Remove))]
        public IActionResult Remove(Guid id)
        {
            _roleAppService.Remove(id);
            return Ok();
        }

        [HttpPost]
        [Permission(nameof(PermissionNames.Role_SetUser))]
        public IActionResult SetUser(SetUserInputDto input)
        {
            _roleAppService.SetUser(input);
            return Ok();
        }

        [HttpPost]
        [Permission(nameof(PermissionNames.Role_SetPermission))]
        public IActionResult SetPermission(SetPermissionInputDto input)
        {
            _roleAppService.SetPermission(input);
            return Ok();
        }
    }

}
