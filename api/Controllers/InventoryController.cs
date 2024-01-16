using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.interfaces;

namespace BeeProject.Controllers;

public class InventoryController : ControllerBase<IService>
{
    public InventoryController(IService inventoryService) : base(inventoryService)
    { }

    //TODO: ValidateAndProceed
    [HttpGet]
    [Authorize]
    [Route("/api/getInventory")]
    public ResponseDto GetAllInventory() =>
        new ResponseDto { 
            MessageToClient = "Successfully fetched every inventory.", 
            ResponseData = Service.GetAllItems<InventoryQuery>("inventory") 
        };

    //TODO: ValidateAndProceed
    [HttpPost]
    [Authorize]
    [ValidateModel]
    [Route("/api/createInventory")]
    public ResponseDto CreateInventory([FromBody] CreateInventoryRequestDto dto) =>
        new ResponseDto
        {
            MessageToClient = "Successfully created an inventory.", 
            ResponseData = Service.CreateItem<InventoryQuery>(
                "inventory",
                new
                {
                    field_Id = dto.FieldId,
                    name = dto.Name,
                    description = dto.Description,
                    amount = dto.Amount
                })
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
            Service.UpdateItem("inventory", inventory);
            return null;
        }, "updated inventory");

    [HttpDelete]
    [Authorize]
    [Route("/api/DeleteInventory/{id:int}")]
    public ResponseDto DeleteInventory([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            Service.DeleteItem("inventory", id); return null;
        }, "deleted inventory");
}
