using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oyang.Identity.Domain;
using Microsoft.AspNetCore.Http;

namespace Oyang.Identity.WebApi.Filters
{
    public class WebApiResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.Filters.Any(t => t is ApiControllerAttribute))
            {
                return;
            }
            if (context.Exception == null)
            {
                return;
            }
            if (context.Exception is DomainException)
            {
                //ValidationProblemDetails
                //BadRequestObjectResult
                //ObjectResult
                //InvalidOperationException
                //System.ComponentModel.DataAnnotations.ValidationException
                //System.ComponentModel.DataAnnotations.EmailAddressAttribute
                //var aaa = new ValidationProblemDetails();
                //var result = new BadRequestObjectResult(context.ModelState);
                //result.ContentTypes.Add(System.Net.Mime.MediaTypeNames.Application.Json);
                var data = new Models.ResultModel()
                {
                    IsSuccess = false,
                    Message = context.Exception.Message,
                };
                context.Result = new ObjectResult(data)
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                };

                context.ExceptionHandled = true;
            }
            else
            {
                var data = new Models.ResultModel()
                {
                    IsSuccess = false,
                    Message = context.Exception.Message,
                };
                context.Result = new ObjectResult(data)
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }
    }
}
