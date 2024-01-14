using BeeProject.Config;
using BeeProject.TransferModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeeProject.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class ControllerBase<TService, TDto> : ControllerBase
    where TDto : class
{
    protected readonly TService Service;

    public ControllerBase(TService service)
    {
        Service = service;
    }

    [NonAction]
    private static bool IsUrlAllowed(string url)
    {
        return Whitelist.AllowedUrls.Any(url.StartsWith);
    }

    [NonAction]
    private static ResponseDto HandleInvalidRequest()
    {
        return new ResponseDto()
        {
            MessageToClient = "Invalid request.",
            ResponseData = null
        };
    }
    [NonAction]
    protected ResponseDto ValidateAndProceed<TResult>(Func<TResult> action, string successMessage)
    {
        if (!ControllerBase<TService, TDto>.IsUrlAllowed(Request.Headers["Referer"]!))
        {
            return HandleInvalidRequest();
        }

        try
        {
            var responseData = action.Invoke();
            return new ResponseDto { MessageToClient = $"Successfully {successMessage}.", ResponseData = responseData };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ResponseDto { MessageToClient = "An error occurred while processing your request." };
        }
    }

}