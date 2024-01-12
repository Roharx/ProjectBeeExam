using BeeProject.Config;
using BeeProject.TransferModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class ControllerBase<TService, TDto> : ControllerBase
    where TService : IService<TDto>
    where TDto : class
{
    private readonly TService _service;

    public ControllerBase(TService service)
    {
        _service = service;
    }

    private bool IsUrlAllowed(string url)
    {
        // Implement your logic here
        return Whitelist.AllowedUrls.Any(url.StartsWith);
    }

    private ResponseDto HandleInvalidRequest()
    {
        return new ResponseDto()
        {
            MessageToClient = "Invalid request.",
            ResponseData = null
        };
    }

    protected ResponseDto ValidateAndProceed(Func<IEnumerable<TDto>> action, string successMessage)
    {
        if (!IsUrlAllowed(Request.Headers["Referer"]!))
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
            // Log the exception or handle it as appropriate for your application
            return new ResponseDto { MessageToClient = "An error occurred while processing your request." };
        }
    }

    // Add more common methods or properties as needed
}