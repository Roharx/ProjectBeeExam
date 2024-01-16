using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;
using service.interfaces;

namespace BeeProject.Controllers;

public class FieldController : ControllerBase<IService>
{
    public FieldController(IService fieldService) : base(fieldService)
    {
    }

    [HttpGet]
    [Authorize]
    [Route("/api/getFields")]
    public ResponseDto GetAllFields() => new ResponseDto
    {
        MessageToClient = "Successfully fetched all fields.", 
        ResponseData = Service.GetAllItems<FieldQuery>("field")
    };

    [HttpGet]
    [Authorize]
    [Route("/api/getAccAccountFieldConnections")]
    public ResponseDto GetAllAccountFieldConnections() => new ResponseDto
    {
        MessageToClient = "Successfully fetched all connections.",
        ResponseData = Service.GetAllItems<Account_FieldQuery>("account_field")
    };

    //TODO: should be in accountController, change when have time or in later update, not that important for now
    [HttpGet]
    [Authorize]
    [Route("/api/getFieldsForAccount/{id:int}")]
    public ResponseDto GetFieldsForAccount([FromRoute] int id) => new ResponseDto
    {
        MessageToClient = "Successfully fetched all fields for account.", 
        ResponseData = Service.GetDataForId<FieldQuery>("account_field", new {account_id = id}, "field")
    };

    [HttpPost]
    [Authorize]
    [ValidateModel]
    [Route("/api/createField")]
    public ResponseDto CreateField([FromBody] CreateFieldRequestDto dto) =>
        new ResponseDto
        {
            MessageToClient = "Successfully created a field.",
            ResponseData = Service.CreateItem<FieldQuery>("field", new
            {
                name = dto.FieldName, 
                location = dto.FieldLocation
            })
        };

    [HttpPut]
    [Authorize]
    [ValidateModel]
    [Route("/api/updateField")]
    public ResponseDto UpdateField([FromBody] UpdateFieldRequestDto dto) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            var field = new 
            {
                id = dto.FieldId, 
                name = dto.FieldName, 
                location = dto.FieldLocation
            };
            Service.UpdateItem("field", field);
            return null;
        }, "updated field");

    [HttpDelete]
    [Authorize]
    [Route("/api/DeleteField/{id:int}")]
    public ResponseDto DeleteField([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            Service.DeleteItem("field", id);
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
            ResponseData = Service.CreateItemWithoutReturn("account_field", 
                new {
                account_id = dto.AccountId, 
                field_id = dto.FieldId
            })
        };

    [HttpPut]
    [Authorize]
    [ValidateModel]
    [Route("/api/DisconnectFieldAndAccount")]
    public ResponseDto DisconnectFieldAndAccount([FromBody] FieldAndAccountDto dto) =>
        new ResponseDto
        {
            MessageToClient = "Successfully disconnected field from account.",
            ResponseData = Service.DeleteItemWithMultipleParams("account_field", 
                new Dictionary<string, object>
            {
                {"account_id", dto.AccountId}, 
                {"field_id", dto.FieldId}
            })
        };
}