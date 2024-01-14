using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

public class FieldController : ControllerBase<FieldService>
{
    public FieldController(FieldService fieldService) : base(fieldService)
    {
    }

    [HttpGet]
    [Authorize]
    [Route("/api/getFields")]
    public ResponseDto GetAllFields() => new ResponseDto
    {
        MessageToClient = "Successfully fetched all fields.", ResponseData = Service.GetAllFields()
    };

    [HttpGet]
    [Authorize]
    [Route("/api/getAccAccountFieldConnections")]
    public ResponseDto GetAllAccountFieldConnections() => new ResponseDto
    {
        MessageToClient = "Successfully fetched all connections.",
        ResponseData = Service.GetAllAccountFieldConnections()
    };

    //TODO: should be in accountController, change when have time or in later update, not that important for now
    [HttpGet]
    [Authorize]
    [Route("/api/getFieldsForAccount/{id:int}")]
    public ResponseDto GetFieldsForAccount([FromRoute] int id) => new ResponseDto
    {
        MessageToClient = "Successfully fetched all fields for account.", ResponseData = Service.GetFieldsForAccount(id)
    };

    [HttpPost]
    [Authorize]
    [ValidateModel]
    [Route("/api/createField")]
    public ResponseDto CreateField([FromBody] CreateFieldRequestDto dto) =>
        new ResponseDto
        {
            MessageToClient = "Successfully created a field.",
            ResponseData = Service.CreateField(dto.FieldName, dto.FieldLocation)
        };

    [HttpPut]
    [Authorize]
    [ValidateModel]
    [Route("/api/updateField")]
    public ResponseDto UpdateField([FromBody] UpdateFieldRequestDto dto) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            var field = new FieldQuery { Id = dto.FieldId, Name = dto.FieldName, Location = dto.FieldLocation };
            Service.UpdateField(field);
            return null;
        }, "updated field");

    //TODO: change to safe later
    [HttpDelete]
    [Authorize]
    [Route("/api/DeleteField/{id:int}")]
    public ResponseDto DeleteField([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            Service.DeleteField(id);
            return null;
        }, "deleted field");

    [HttpPost]
    [Authorize]
    [ValidateModel]
    [Route("/api/ConnectFieldAndAccount")]
    public ResponseDto ConnectFieldAndAccount([FromBody] FieldAndAccountDto dto) =>
        new ResponseDto
        {
            MessageToClient = "Successfully connected field to account.",
            ResponseData = Service.ConnectFieldAndAccount(dto.AccountId, dto.FieldId)
        };

    [HttpPut]
    [Authorize]
    [ValidateModel]
    [Route("/api/DisconnectFieldAndAccount")]
    public ResponseDto DisconnectFieldAndAccount([FromBody] FieldAndAccountDto dto) =>
        new ResponseDto
        {
            MessageToClient = "Successfully disconnected field from account.",
            ResponseData = Service.DisconnectFieldAndAccount(dto.AccountId, dto.FieldId)
        };
}