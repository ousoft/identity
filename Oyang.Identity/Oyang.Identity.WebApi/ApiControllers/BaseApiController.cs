using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oyang.Identity.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Oyang.Identity.WebApi.ApiControllers
{
    [Authorize]
    //[ApiController]
    [Route("api/[Controller]/[Action]")]
    public abstract class BaseApiController : ControllerBase
    {

    }

}
