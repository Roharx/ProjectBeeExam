using BeeProject.TransferModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeeProject.Filters;

public class ValidateModel : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
            return;
        string errorMessages = context.ModelState
            .Values
            .SelectMany(i => i.Errors.Select(e => e.ErrorMessage))
            .Aggregate((i, j) => i + "," + j);
        context.Result = new JsonResult(new ResponseDto
        {
            MessageToClient = errorMessages
        })
        {
            StatusCode = 400
        };
    }
}