using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.interfaces;

namespace BeeProject.Controllers;

public class HoneyController : ControllerBase<IService>
{
    public HoneyController(IService honeyService) : base(honeyService)
    { }

    //TODO: ValidateAndProceed
    [HttpGet]
    [Authorize]
    [Route("/api/getHoney")]
    public ResponseDto GetAllHoneys() =>
        new ResponseDto
            { 
                MessageToClient = "Successfully fetched every honey.", 
                ResponseData = Service.GetAllItems<HoneyQuery>("honey") 
            };

    //TODO: ValidateAndProceed
    [HttpPost]
    [Authorize]
    [ValidateModel]
    [Route("/api/createHoney")]
    public ResponseDto CreateHoney([FromBody] CreateHoneyRequestDto dto) =>
        new ResponseDto
        {
            MessageToClient = "Successfully created a honey.",
            ResponseData = Service.CreateItem<HoneyQuery>(
                "honey",
                new
                {
                    name = dto.Name,
                    liquid = dto.Liquid,
                    harvest_id = dto.Harvest,
                    moisture = dto.Moisture,
                    flowers = dto.Flowers
                })
        };

    [HttpPut]
    [Authorize]
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
            Service.UpdateItem("honey", honey);
            return null;
        }, "updated honey");

    [HttpDelete]
    [Authorize]
    [Route("/api/DeleteHoney/{id:int}")]
    public ResponseDto DeleteHoney([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            Service.DeleteItem("honey", id);
            return null;
        }, "deleted honey");
}