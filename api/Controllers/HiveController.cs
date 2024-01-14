using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

public class HiveController : ControllerBase<HiveService>
{
    public HiveController(HiveService hiveService): base(hiveService)
    { }

    [HttpGet]
    [Authorize]
    [Route("/api/getHives")]
    public ResponseDto GetAllHives() => new ResponseDto
    {
        MessageToClient = "Successfully fetched all hives.", ResponseData = Service.GetAllHives()
    };

    [HttpGet]
    [Authorize]
    [Route("/api/getHivesForField/{id:int}")]
    public ResponseDto GetAllHivesForField([FromRoute] int id) =>
        new ResponseDto
        {
            MessageToClient = "Successfully fetched all hives.", ResponseData = Service.GetHivesForField(id)
        };

    [HttpPost]
    [Authorize]
    [ValidateModel]
    [Route("/api/createHive")]
    public ResponseDto CreateHive([FromBody] CreateHiveRequestDto dto) =>
        new ResponseDto { 
            MessageToClient = "Successfully created a hive.", 
            ResponseData = Service.CreateHive(
                dto.FieldId, 
                dto.Name, 
                dto.Location, 
                dto.PlacementDate, 
                dto.LastCheck, 
                dto.ReadyToHarvest, 
                dto.Color, 
                dto.Comment!, 
                dto.BeeId) 
        };

    [HttpPut]
    [Authorize]
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
            Service.UpdateHive(hive);
            return null;
        }, "updated hive");

    [HttpDelete]
    [Authorize]
    [Route("/api/DeleteHive/{id:int}")]
    public ResponseDto DeleteHive([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            Service.DeleteHive(id); return null;
        }, "deleted hive");
}
