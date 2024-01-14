using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.DataModels.Enums;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

public class AilmentController : ControllerBase<AilmentService>
{

    public AilmentController(AilmentService ailmentService) : base(ailmentService)
    { }
    
    [HttpGet]
    [Authorize]
    [Route("/api/getAilments")]
    public ResponseDto GetAllAilments() => new ResponseDto 
        { MessageToClient = "Successfully fetched all ailments.", ResponseData = Service.GetAllAilments() };

    [HttpGet]
    [Authorize]
    [Route("/api/getGlobalAilments")]
    public ResponseDto GetGlobalAilments() => new ResponseDto 
        { MessageToClient = "Successfully fetched all ailments.", ResponseData = Service.GetGlobalAilments() };

    [HttpPost]
    [Authorize]
    [ValidateModel]
    [Route("/api/createAilment")]
    public ResponseDto CreateAilment([FromBody] CreateAilmentRequestDto dto) =>
        new ResponseDto { MessageToClient = "Successfully created an ailment.", ResponseData = Service.CreateAilment(dto.Hive_Id, dto.Name, (AilmentSeverity)Enum.ToObject(typeof(AilmentSeverity), dto.Severity), dto.Solved, dto.Comment!) };

    [HttpPut]
    [Authorize]
    [ValidateModel]
    [Route("/api/updateAilment")]
    public ResponseDto UpdateAilment([FromBody] UpdateAilmentRequestDto dto)
    {
        var ailment = new AilmentQuery
        {
            Id = dto.Id,
            Hive_Id = dto.HiveId,
            Name = dto.Name,
            Severity = dto.Severity,
            Comment = dto.Comment,
            Solved = dto.Solved
        };
        Service.UpdateAilment(ailment);
        return new ResponseDto { MessageToClient = "Successfully updated ailment." };
    }

    [HttpDelete]
    [Authorize]
    [Route("/api/DeleteAilment/{id:int}")]
    public ResponseDto DeleteAilment([FromRoute] int id)
    {
        Service.DeleteAilment(id);
        return new ResponseDto { MessageToClient = "Successfully deleted ailment." };
    }
}
