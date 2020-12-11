using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oyang.Identity.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Oyang.Identity.IApplication.User;
using Oyang.Identity.IApplication.User.Dtos;
using Oyang.Identity.Infrastructure.Common;

namespace Oyang.Identity.WebApi.ApiControllers
{
    public class UserController : BaseApiController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserAppService  _userAppService;

        public UserController(
            ILogger<UserController> logger,
            IUserAppService userAppService
            )
        {
            _logger = logger;
            _userAppService = userAppService;
        }

        [HttpGet]
        [Permission(nameof(PermissionNames.User_GetList))]
        public ActionResult<Pagination<UserDto>> GetList(GetListInputDto input)
        {
            return _userAppService.GetList(input);
        }

        [HttpGet("{id}")]
        [Permission(nameof(PermissionNames.User_Get))]
        public IActionResult Get(Guid id)
        {
            var model = _userAppService.Get(id);
            return Ok(model);
        }

        [HttpPost]
        [Permission(nameof(PermissionNames.User_Add))]
        public IActionResult Add(AddInputDto input)
        {
            _userAppService.Add(input);
            return Ok();
        }

        [HttpPost]
        [Permission(nameof(PermissionNames.User_Update))]
        public IActionResult Update(UpdateInputDto input)
        {
            _userAppService.Update(input);
            return Ok();
        }

        [HttpPost("{id}")]
        [Permission(nameof(PermissionNames.User_Remove))]
        public IActionResult Remove(Guid id)
        {
            _userAppService.Remove(id);
            return Ok();
        }

        [HttpPost]
        [Permission(nameof(PermissionNames.User_ResetPassword))]
        public IActionResult ResetPassword(ResetPasswordInputDto input)
        {
            _userAppService.ResetPassword(input);
            return Ok();
        }

        [HttpPost]
        [Permission(nameof(PermissionNames.User_SetRole))]
        public IActionResult SetRole(SetRoleInputDto input)
        {
            _userAppService.SetRole(input);
            return Ok();
        }

    }

}
