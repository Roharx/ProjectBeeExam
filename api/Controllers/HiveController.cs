using BeeProject.Config;
using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

// TODO: convert dates and times, probably easier to handle as a string
public class HiveController : ControllerBase
{
    private readonly HiveService _hiveService;

    public HiveController(HiveService hiveService)
    {
        _hiveService = hiveService;
    }

    private bool IsUrlAllowed(string url) => Whitelist.AllowedUrls.Any(url.StartsWith);

    private ResponseDto HandleInvalidRequest() => new ResponseDto { MessageToClient = "Invalid request.", ResponseData = null };

    private ResponseDto ValidateAndProceed<T>(Func<T> action, string successMessage) =>
        !IsUrlAllowed(Request.Headers["Referer"]) ? HandleInvalidRequest() : new ResponseDto { MessageToClient = $"Successfully {successMessage}.", ResponseData = action.Invoke() };

    [HttpGet]
    [Route("/api/getHives")]
    public ResponseDto GetAllHives() => new ResponseDto { MessageToClient = "Successfully fetched all hives.", ResponseData = _hiveService.GetAllHives() };

    [HttpGet]
    [Route("/api/getHivesForField/{id:int}")]
    public ResponseDto GetAllHivesForField([FromRoute] int id) =>
        new ResponseDto { MessageToClient = "Successfully fetched all hives.", ResponseData = _hiveService.GetHivesForField(id) };

    [HttpPost]
    [ValidateModel]
    [Route("/api/createHive")]
    public ResponseDto CreateHive([FromBody] CreateHiveRequestDto dto) =>
        new ResponseDto { MessageToClient = "Successfully created a hive.", ResponseData = _hiveService.CreateHive(dto.FieldId, dto.Name, dto.Location, dto.PlacementDate, dto.LastCheck, dto.ReadyToHarvest, dto.Color, dto.Comment!, dto.BeeId) };

    [HttpPut]
    [ValidateModel]
    [Route("/api/updateHive")]
    public ResponseDto UpdateHive([FromBody] UpdateHiveRequestDto dto) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            var hive = new HiveQuery
            {
                Id = dto.Id,
                Field_Id = dto.Field_Id,
                Name = dto.Name,
                Location = dto.Location,
                Placement = dto.Placement,
                Last_Check = dto.Last_Check,
                Ready = dto.Ready,
                Color = dto.Color,
                Bee_Type = dto.Bee_Type,
                Comment = dto.Comment
            };
            _hiveService.UpdateHive(hive);
            return null;
        }, "updated hive");

    [HttpDelete]
    [Route("/api/DeleteHive/{id:int}")]
    public ResponseDto DeleteHive([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() => { _hiveService.DeleteHive(id); return null; }, "deleted hive");
}
