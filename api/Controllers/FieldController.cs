using BeeProject.Config;
using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

public class FieldController : ControllerBase
{
    private readonly FieldService _fieldService;

    public FieldController(FieldService fieldService)
    {
        _fieldService = fieldService;
    }

    private bool IsUrlAllowed(string url) => Whitelist.AllowedUrls.Any(url.StartsWith);

    private ResponseDto HandleInvalidRequest() => new ResponseDto { MessageToClient = "Invalid request.", ResponseData = null };

    private ResponseDto ValidateAndProceed<T>(Func<T> action, string successMessage) =>
        !IsUrlAllowed(Request.Headers["Referer"]) ? HandleInvalidRequest() : new ResponseDto { MessageToClient = $"Successfully {successMessage}.", ResponseData = action.Invoke() };

    [HttpGet]
    [Route("/api/getFields")]
    public ResponseDto GetAllFields() => new ResponseDto { MessageToClient = "Successfully fetched all fields.", ResponseData = _fieldService.GetAllFields() };

    [HttpGet]
    [Route("/api/getAccAccountFieldConnections")]
    public ResponseDto GetAllAccountFieldConnections() => new ResponseDto { MessageToClient = "Successfully fetched all connections.", ResponseData = _fieldService.GetAllAccountFieldConnections() };

    //TODO: should be in accountController, change when have time or in later update, not that important for now
    [HttpGet]
    [Route("/api/getFieldsForAccount/{id:int}")]
    public ResponseDto GetFieldsForAccount([FromRoute] int id) => new ResponseDto { MessageToClient = "Successfully fetched all fields for account.", ResponseData = _fieldService.GetFieldsForAccount(id) };

    [HttpPost]
    [ValidateModel]
    [Route("/api/createField")]
    public ResponseDto CreateField([FromBody] CreateFieldRequestDto dto) =>
        new ResponseDto { MessageToClient = "Successfully created a field.", ResponseData = _fieldService.CreateField(dto.FieldName, dto.FieldLocation) };

    [HttpPut]
    [ValidateModel]
    [Route("/api/updateField")]
    public ResponseDto UpdateField([FromBody] UpdateFieldRequestDto dto) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            var field = new FieldQuery { Id = dto.FieldId, Name = dto.FieldName, Location = dto.FieldLocation };
            _fieldService.UpdateField(field);
            return null;
        }, "updated field");

    //TODO: change to safe later
    [HttpDelete]
    [Route("/api/DeleteField/{id:int}")]
    public ResponseDto DeleteField([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() => { _fieldService.DeleteField(id); return null; }, "deleted field");

    [HttpPost]
    [ValidateModel]
    [Route("/api/ConnectFieldAndAccount")]
    public ResponseDto ConnectFieldAndAccount([FromBody] FieldAndAccountDto dto) =>
        new ResponseDto { MessageToClient = "Successfully connected field to account.", ResponseData = _fieldService.ConnectFieldAndAccount(dto.AccountId, dto.FieldId) };

    [HttpPut]
    [ValidateModel]
    [Route("/api/DisconnectFieldAndAccount")]
    public ResponseDto DisconnectFieldAndAccount([FromBody] FieldAndAccountDto dto) =>
        new ResponseDto { MessageToClient = "Successfully disconnected field from account.", ResponseData = _fieldService.DisconnectFieldAndAccount(dto.AccountId, dto.FieldId) };
}
