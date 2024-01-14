using BeeProject.Config;
using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

public class BeeController : ControllerBase<BeeService, BeeQuery>
{
    public BeeController(BeeService beeService) : base(beeService)
    { }
    
    [HttpGet]
    [Authorize]
    [Route("/api/getBees")]
    public ResponseDto GetAllBees() => new ResponseDto { MessageToClient = "Successfully fetched all bees.", ResponseData = Service.GetAllBees() };

    [HttpPost]
    [Authorize]
    [ValidateModel]
    [Route("/api/createBee")]
    public ResponseDto CreateBee([FromBody] CreateBeeRequestDto dto) =>
        new ResponseDto { MessageToClient = "Successfully created a bee.", ResponseData = Service.CreateBee(dto.Name, dto.Description, dto.Comment!) };

    [HttpPut]
    [Authorize]
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
            Service.UpdateBee(bee);
            return null;
        }, "updated bee");

    //TODO: change to safe later
    [HttpDelete]
    [Authorize]
    [Route("/api/DeleteBee/{id:int}")]
    public ResponseDto DeleteBee([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() => { Service.DeleteBee(id); return null; }, "deleted bee");
}
