using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.interfaces;

namespace BeeProject.Controllers;

public class HarvestController : ControllerBase<IService>
{
    public HarvestController(IService harvestService) : base(harvestService)
    { }
    //TODO: ValidateAndProceed
    [HttpGet]
    [Authorize]
    [Route("/api/getHarvests")]
    public ResponseDto GetAllHarvests() => new ResponseDto
    {
        MessageToClient = "Successfully fetched all harvests.", 
        ResponseData = Service.GetAllItems<HarvestQuery>("harvest")
    };
    //TODO: ValidateAndProceed
    [HttpPost]
    [Authorize]
    [ValidateModel]
    [Route("/api/createHarvest")]
    public ResponseDto CreateHarvest([FromBody] CreateHarvestRequestDto dto) =>
        new ResponseDto
        {
            MessageToClient = "Successfully created a harvest.", 
            ResponseData = Service.CreateItem<HarvestQuery>("harvest",
                new
                {
                    hive_id = dto.HiveId, 
                    time = dto.Time, 
                    honey_amount = dto.HoneyAmount, 
                    beeswax_amount = dto.BeeswaxAmount, 
                    comment = dto.Comment
                }
                )
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
            Service.UpdateItem("harvest", harvest);
            return null;
        }, "updated harvest");

    [HttpDelete]
    [Authorize]
    [Route("/api/DeleteHarvest/{id:int}")]
    public ResponseDto DeleteHarvest([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            Service.DeleteItem("harvest", id); return null;
        }, "deleted harvest");
}
