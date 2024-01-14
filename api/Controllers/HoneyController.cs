using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

public class HoneyController : ControllerBase<HoneyService>
{
    public HoneyController(HoneyService honeyService) : base(honeyService)
    { }

    [HttpGet]
    [Authorize]
    [Route("/api/getHoney")]
    public ResponseDto GetAllHoneys() =>
        new ResponseDto
            { MessageToClient = "Successfully fetched every honey.", ResponseData = Service.GetAllHoney() };

    [HttpPost]
    [Authorize]
    [ValidateModel]
    [Route("/api/createHoney")]
    public ResponseDto CreateHoney([FromBody] CreateHoneyRequestDto dto) =>
        new ResponseDto
        {
            MessageToClient = "Successfully created a honey.",
            ResponseData = Service.CreateHoney(dto.Harvest, dto.Name, dto.Liquid, dto.Flowers, dto.Moisture)
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
            Service.UpdateHoney(honey);
            return null;
        }, "updated honey");

    [HttpDelete]
    [Authorize]
    [Route("/api/DeleteHoney/{id:int}")]
    public ResponseDto DeleteHoney([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            Service.DeleteHoney(id);
            return null;
        }, "deleted honey");
}