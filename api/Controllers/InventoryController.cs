using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

public class InventoryController : ControllerBase<InventoryService>
{
    public InventoryController(InventoryService inventoryService) : base(inventoryService)
    { }

    [HttpGet]
    [Authorize]
    [Route("/api/getInventory")]
    public ResponseDto GetAllInventory() =>
        new ResponseDto { 
            MessageToClient = "Successfully fetched every inventory.", 
            ResponseData = Service.GetAllInventoryItems() 
        };

    [HttpPost]
    [Authorize]
    [ValidateModel]
    [Route("/api/createInventory")]
    public ResponseDto CreateInventory([FromBody] CreateInventoryRequestDto dto) =>
        new ResponseDto
        {
            MessageToClient = "Successfully created an inventory.", 
            ResponseData = Service.CreateInventoryItem(
                dto.FieldId, 
                dto.Name, 
                dto.Description, 
                dto.Amount)
        };

    [HttpPut]
    [Authorize]
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
            Service.UpdateInventoryItem(inventory);
            return null;
        }, "updated inventory");

    [HttpDelete]
    [Authorize]
    [Route("/api/DeleteInventory/{id:int}")]
    public ResponseDto DeleteInventory([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            Service.DeleteInventoryItem(id); return null;
        }, "deleted inventory");
}
