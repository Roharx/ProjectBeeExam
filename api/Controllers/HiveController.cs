using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.interfaces;

namespace BeeProject.Controllers;

public class HiveController : ControllerBase<IService>
{
    public HiveController(IService hiveService): base(hiveService)
    { }

    //TODO: ValidateAndProceed
    [HttpGet]
    [Authorize]
    [Route("/api/getHives")]
    public ResponseDto GetAllHives() => new ResponseDto
    {
        MessageToClient = "Successfully fetched all hives.", 
        ResponseData = Service.GetAllItems<HiveQuery>("hive")
    };

    //TODO: ValidateAndProceed
    [HttpGet]
    [Authorize]
    [Route("/api/getHivesForField/{id:int}")]
    public ResponseDto GetAllHivesForField([FromRoute] int id) =>
        new ResponseDto
        {
            MessageToClient = "Successfully fetched all hives.", 
            ResponseData = Service.GetItemsByParameters<HarvestQuery>("hive", id)
        };

    //TODO: ValidateAndProceed
    [HttpPost]
    [Authorize]
    [ValidateModel]
    [Route("/api/createHive")]
    public ResponseDto CreateHive([FromBody] CreateHiveRequestDto dto) =>
        new ResponseDto { 
            MessageToClient = "Successfully created a hive.", 
            ResponseData = Service.CreateItem<HiveQuery>(
                "hive",
                new
                {
                    field_id = dto.FieldId, 
                    name = dto.Name, 
                    location = dto.Location, 
                    placement = dto.PlacementDate, 
                    last_check = dto.LastCheck, 
                    ready = dto.ReadyToHarvest, 
                    color = dto.Color, 
                    comment = dto.Comment!, 
                    bee_type = dto.BeeId
                }) 
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
            Service.UpdateItem("hive", hive);
            return null;
        }, "updated hive");

    [HttpDelete]
    [Authorize]
    [Route("/api/DeleteHive/{id:int}")]
    public ResponseDto DeleteHive([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            Service.DeleteItem("hive", id); return null;
        }, "deleted hive");
}
