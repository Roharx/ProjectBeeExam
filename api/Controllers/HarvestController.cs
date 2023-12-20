using BeeProject.Config;
using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

public class HarvestController : ControllerBase
{
    private readonly HarvestService _harvestService;

    public HarvestController(HarvestService harvestService)
    {
        _harvestService = harvestService;
    }

    private bool IsUrlAllowed(string url) => Whitelist.AllowedUrls.Any(url.StartsWith);

    private ResponseDto HandleInvalidRequest() => new ResponseDto { MessageToClient = "Invalid request.", ResponseData = null };

    private ResponseDto ValidateAndProceed<T>(Func<T> action, string successMessage) =>
        !IsUrlAllowed(Request.Headers["Referer"]) ? HandleInvalidRequest() : new ResponseDto { MessageToClient = $"Successfully {successMessage}.", ResponseData = action.Invoke() };

    [HttpGet]
    [Route("/api/getHarvests")]
    public ResponseDto GetAllHarvests() => new ResponseDto { MessageToClient = "Successfully fetched all harvests.", ResponseData = _harvestService.GetAllHarvests() };

    [HttpPost]
    [ValidateModel]
    [Route("/api/createHarvest")]
    public ResponseDto CreateHarvest([FromBody] CreateHarvestRequestDto dto) =>
        new ResponseDto { MessageToClient = "Successfully created a harvest.", ResponseData = _harvestService.CreateHarvest(dto.HiveId, dto.Time, dto.HoneyAmount, dto.BeeswaxAmount, dto.Comment) };

    [HttpPut]
    [ValidateModel]
    [Route("/api/updateHarvest")]
    public ResponseDto UpdateHarvest([FromBody] UpdateHarvestRequestDto dto) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            var harvest = new HarvestQuery { Id = dto.Id, Hive_Id = dto.HiveId, Time = dto.Time, Honey_Amount = dto.HoneyAmount, Beeswax_Amount = dto.BeeswaxAmount, Comment = dto.Comment };
            _harvestService.UpdateHarvest(harvest);
            return null;
        }, "updated harvest");

    //TODO: change to safe later
    [HttpDelete]
    [Route("/api/DeleteHarvest/{id:int}")]
    public ResponseDto DeleteHarvest([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() => { _harvestService.DeleteHarvest(id); return null; }, "deleted harvest");
}
