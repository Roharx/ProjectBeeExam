using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers;

public class TaskController : ControllerBase<TaskService>
{
    public TaskController(TaskService taskService) : base(taskService)
    { }

    [HttpGet]
    [Authorize]
    [Route("/api/getTask")]
    public ResponseDto GetAllTask() =>
        new ResponseDto
        {
            MessageToClient = "Successfully fetched every task.", 
            ResponseData = Service.GetAllTasks()
        };

    [HttpPost]
    [Authorize]
    [ValidateModel]
    [Route("/api/createTask")]
    public ResponseDto CreateTask([FromBody] CreateTaskRequestDto dto) =>
        new ResponseDto
        {
            MessageToClient = "Successfully created a task.", 
            ResponseData = Service.CreateTask(dto.HiveId, dto.Name, dto.Description!, dto.Done)
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
            Service.UpdateTask(task);
            return null;
        }, "updated task");

    // TODO: change to safe later
    [HttpDelete]
    [Authorize]
    [Route("/api/DeleteTask/{id:int}")]
    public ResponseDto DeleteTask([FromRoute] int id) =>
        ValidateAndProceed<ResponseDto>(() =>
        {
            Service.DeleteTask(id); return null;
        }, "deleted task");
}
