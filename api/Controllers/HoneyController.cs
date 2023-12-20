using BeeProject.Config;
using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

public class HoneyController : ControllerBase
{
    private readonly HoneyService _honeyService;

    public HoneyController(HoneyService honeyService)
    {
        _honeyService = honeyService;
    }

    private bool IsUrlAllowed(string url) => Whitelist.AllowedUrls.Any(url.StartsWith);

    private ResponseDto HandleInvalidRequest() => new ResponseDto { MessageToClient = "Invalid request.", ResponseData = null };

    private ResponseDto ValidateAndProceed<T>(Func<T> action, string successMessage) =>
        !IsUrlAllowed(Request.Headers["Referer"]) ? HandleInvalidRequest() : new ResponseDto { MessageToClient = $"Successfully {successMessage}.", ResponseData = action.Invoke() };

    [HttpGet]
    [Route("/api/getHoney")]
    public ResponseDto GetAllHoneys() =>
        new ResponseDto { MessageToClient = "Successfully fetched every honey.", ResponseData = _honeyService.GetAllHoney() };

    [HttpPost]
    [ValidateModel]
    [Route("/api/createHoney")]
    public ResponseDto CreateHoney([FromBody] CreateHoneyRequestDto dto) =>
        new ResponseDto { MessageToClient = "Successfully created a honey.", ResponseData = _honeyService.CreateHoney(dto.Harvest, dto.Name, dto.Liquid, dto.Flowers, dto.Moisture) };

    [HttpPut]
    [ValidateModel]
    [Route("/api/updateHoney")]
    public ResponseDto UpdateHoney([FromBody] UpdateHoneyRequestDto dto) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            var honey = new HoneyQuery
            {
                Id = dto.Id,
                Name = dto.Name,
                Liquid = dto.Liquid,
                Harvest_id = dto.Harvest,
                Moisture = dto.Moisture,
                Flowers = dto.Flowers,
            };
            _honeyService.UpdateHoney(honey);
            return null;
        }, "updated honey");

    // TODO: change to safe later
    [HttpDelete]
    [Route("/api/DeleteHoney/{id:int}")]
    public ResponseDto DeleteHoney([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() => { _honeyService.DeleteHoney(id); return null; }, "deleted honey");
}
