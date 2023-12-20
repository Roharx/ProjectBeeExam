using BeeProject.Config;
using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

public class InventoryController : ControllerBase
{
    private readonly InventoryService _inventoryService;

    public InventoryController(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    private bool IsUrlAllowed(string url) => Whitelist.AllowedUrls.Any(url.StartsWith);

    private ResponseDto HandleInvalidRequest() => new ResponseDto { MessageToClient = "Invalid request.", ResponseData = null };

    private ResponseDto ValidateAndProceed<T>(Func<T> action, string successMessage) =>
        !IsUrlAllowed(Request.Headers["Referer"]) ? HandleInvalidRequest() : new ResponseDto { MessageToClient = $"Successfully {successMessage}.", ResponseData = action.Invoke() };

    [HttpGet]
    [Route("/api/getInventory")]
    public ResponseDto GetAllInventory() =>
        new ResponseDto { MessageToClient = "Successfully fetched every inventory.", ResponseData = _inventoryService.GetAllInventoryItems() };

    [HttpPost]
    [ValidateModel]
    [Route("/api/createInventory")]
    public ResponseDto CreateInventory([FromBody] CreateInventoryRequestDto dto) =>
        new ResponseDto { MessageToClient = "Successfully created an inventory.", ResponseData = _inventoryService.CreateInventoryItem(dto.FieldId, dto.Name, dto.Description, dto.Amount) };

    [HttpPut]
    [ValidateModel]
    [Route("/api/updateInventory")]
    public ResponseDto UpdateInventory([FromBody] UpdateInventoryRequestDto dto) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            var inventory = new InventoryQuery
            {
                Id = dto.Id,
                Field_Id = dto.FieldId,
                Name = dto.Name,
                Description = dto.Description,
                Amount = dto.Amount
            };
            _inventoryService.UpdateInventoryItem(inventory);
            return null;
        }, "updated inventory");

    [HttpDelete]
    [Route("/api/DeleteInventory/{id:int}")]
    public ResponseDto DeleteInventory([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() => { _inventoryService.DeleteInventoryItem(id); return null; }, "deleted inventory");
}
