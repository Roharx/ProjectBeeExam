using BeeProject.Config;
using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.DataModels.Enums;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

public class AilmentController : ControllerBase
{
    private readonly AilmentService _ailmentService;

    public AilmentController(AilmentService ailmentService)
    {
        _ailmentService = ailmentService;
    }

    private bool IsUrlAllowed(string url) => Whitelist.AllowedUrls.Any(url.StartsWith);

    private ResponseDto HandleInvalidRequest() => new ResponseDto { MessageToClient = "Invalid request.", ResponseData = null };

    private ResponseDto ValidateAndProceed<T>(Func<T> action, string successMessage) =>
        !IsUrlAllowed(Request.Headers["Referer"]) ? HandleInvalidRequest() : new ResponseDto { MessageToClient = $"Successfully {successMessage}.", ResponseData = action.Invoke() };

    [HttpGet]
    [Route("/api/getAilments")]
    public ResponseDto GetAllAilments() => new ResponseDto { MessageToClient = "Successfully fetched all ailments.", ResponseData = _ailmentService.GetAllAilments() };

    [HttpGet]
    [Route("/api/getGlobalAilments")]
    public ResponseDto GetGlobalAilments() => new ResponseDto { MessageToClient = "Successfully fetched all ailments.", ResponseData = _ailmentService.GetGlobalAilments() };

    [HttpPost]
    [ValidateModel]
    [Route("/api/createAilment")]
    public ResponseDto CreateAilment([FromBody] CreateAilmentRequestDto dto) =>
        new ResponseDto { MessageToClient = "Successfully created an ailment.", ResponseData = _ailmentService.CreateAilment(dto.Hive_Id, dto.Name, (AilmentSeverity)Enum.ToObject(typeof(AilmentSeverity), dto.Severity), dto.Solved, dto.Comment!) };

    [HttpPut]
    [ValidateModel]
    [Route("/api/updateAilment")]
    public ResponseDto UpdateAilment([FromBody] UpdateAilmentRequestDto dto)
    {
        var ailment = new AilmentQuery
        {
            Id = dto.Id,
            Hive_Id = dto.HiveId,
            Name = dto.Name,
            Severity = dto.Severity,
            Comment = dto.Comment,
            Solved = dto.Solved
        };
        _ailmentService.UpdateAilment(ailment);
        return new ResponseDto { MessageToClient = "Successfully updated ailment." };
    }

    //TODO: change to safe later
    [HttpDelete]
    [Route("/api/DeleteAilment/{id:int}")]
    public ResponseDto DeleteAilment([FromRoute] int id)
    {
        _ailmentService.DeleteAilment(id);
        return new ResponseDto { MessageToClient = "Successfully deleted ailment." };
    }
}
