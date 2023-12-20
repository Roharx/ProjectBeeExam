using BeeProject.Config;
using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

public class BeeController : ControllerBase
{
    private readonly BeeService _beeService;

    public BeeController(BeeService beeService)
    {
        _beeService = beeService;
    }

    private bool IsUrlAllowed(string url) => Whitelist.AllowedUrls.Any(url.StartsWith);

    private ResponseDto HandleInvalidRequest() => new ResponseDto { MessageToClient = "Invalid request.", ResponseData = null };

    private ResponseDto ValidateAndProceed<T>(Func<T> action, string successMessage) =>
        !IsUrlAllowed(Request.Headers["Referer"]) ? HandleInvalidRequest() : new ResponseDto { MessageToClient = $"Successfully {successMessage}.", ResponseData = action.Invoke() };

    [HttpGet]
    [Route("/api/getBees")]
    public ResponseDto GetAllBees() => new ResponseDto { MessageToClient = "Successfully fetched all bees.", ResponseData = _beeService.GetAllBees() };

    [HttpPost]
    [ValidateModel]
    [Route("/api/createBee")]
    public ResponseDto CreateBee([FromBody] CreateBeeRequestDto dto) =>
        new ResponseDto { MessageToClient = "Successfully created a bee.", ResponseData = _beeService.CreateBee(dto.Name, dto.Description, dto.Comment!) };

    [HttpPut]
    [ValidateModel]
    [Route("/api/updateBee")]
    public ResponseDto UpdateBee([FromBody] UpdateBeeRequestDto dto) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            var bee = new BeeQuery
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Comment = dto.Comment
            };
            _beeService.UpdateBee(bee);
            return null;
        }, "updated bee");

    //TODO: change to safe later
    [HttpDelete]
    [Route("/api/DeleteBee/{id:int}")]
    public ResponseDto DeleteBee([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() => { _beeService.DeleteBee(id); return null; }, "deleted bee");
}
