using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

public class HarvestController : ControllerBase<HarvestService>
{
    public HarvestController(HarvestService harvestService) : base(harvestService)
    { }
    
    [HttpGet]
    [Authorize]
    [Route("/api/getHarvests")]
    public ResponseDto GetAllHarvests() => new ResponseDto
    {
        MessageToClient = "Successfully fetched all harvests.", ResponseData = Service.GetAllHarvests()
    };

    [HttpPost]
    [Authorize]
    [ValidateModel]
    [Route("/api/createHarvest")]
    public ResponseDto CreateHarvest([FromBody] CreateHarvestRequestDto dto) =>
        new ResponseDto
        {
            MessageToClient = "Successfully created a harvest.", 
            ResponseData = Service.CreateHarvest(dto.HiveId, dto.Time, dto.HoneyAmount, dto.BeeswaxAmount, dto.Comment)
        };

    [HttpPut]
    [Authorize]
    [ValidateModel]
    [Route("/api/updateHarvest")]
    public ResponseDto UpdateHarvest([FromBody] UpdateHarvestRequestDto dto) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            var harvest = new HarvestQuery
            {
                Id = dto.Id, 
                Hive_Id = dto.HiveId, 
                Time = dto.Time, 
                Honey_Amount = dto.HoneyAmount, 
                Beeswax_Amount = dto.BeeswaxAmount, 
                Comment = dto.Comment
            };
            Service.UpdateHarvest(harvest);
            return null;
        }, "updated harvest");

    //TODO: change to safe later
    [HttpDelete]
    [Authorize]
    [Route("/api/DeleteHarvest/{id:int}")]
    public ResponseDto DeleteHarvest([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            Service.DeleteHarvest(id); return null;
        }, "deleted harvest");
}
