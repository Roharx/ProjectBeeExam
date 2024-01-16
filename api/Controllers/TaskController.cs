using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.interfaces;

namespace BeeProject.Controllers;

public class TaskController : ControllerBase<IService>
{
    public TaskController(IService taskService) : base(taskService)
    { }

    //TODO: ValidateAndProceed
    [HttpGet]
    [Authorize]
    [Route("/api/getTask")]
    public ResponseDto GetAllTask() =>
        new ResponseDto
        {
            MessageToClient = "Successfully fetched every task.", 
            ResponseData = Service.GetAllItems<TaskQuery>("task")
        };

    //TODO: ValidateAndProceed
    [HttpPost]
    [Authorize]
    [ValidateModel]
    [Route("/api/createTask")]
    public ResponseDto CreateTask([FromBody] CreateTaskRequestDto dto) =>
        new ResponseDto
        {
            MessageToClient = "Successfully created a task.", 
            ResponseData = Service.CreateItem<TaskQuery>(
                "task",
                new
                {
                    hive_Id = dto.HiveId,
                    name = dto.Name,
                    description = dto.Description,
                    done = dto.Done
                })
        };

    [HttpPut]
    [Authorize]
    [ValidateModel]
    [Route("/api/updateTask")]
    public ResponseDto UpdateTask([FromBody] UpdateTaskRequestDto dto) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            var task = new TaskQuery
            {
                Id = dto.Id,
                Hive_Id = dto.HiveId,
                Name = dto.Name,
                Description = dto.Description,
                Done = dto.Done
            };
            Service.UpdateItem("task", task);
            return null;
        }, "updated task");

    [HttpDelete]
    [Authorize]
    [Route("/api/DeleteTask/{id:int}")]
    public ResponseDto DeleteTask([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            Service.DeleteItem("task", id); return null;
        }, "deleted task");
}
