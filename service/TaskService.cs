using infrastructure.Interfaces;
using infrastructure.QueryModels;
using infrastructure.Repositories;

namespace service;

public class TaskService
{
    private readonly IRepository _repository;
    public TaskService(IRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<TaskQuery> GetAllTasks()
    {
        return _repository.GetAllItems<TaskQuery>("task");
    }

    public int CreateTask(int hiveId, string taskName, string taskDescription, bool taskDone)
    {
        var parameters = new
        {
            hive_id = hiveId,
            name = taskName,
            description = taskDescription,
            done = taskDone
        };
        var result = _repository.CreateItem<int>("task", parameters);
        return result != -1 ? result : throw new Exception("Could not create task.");
    }

    public void UpdateTask(TaskQuery task)
    {
        if (!_repository.UpdateEntity("task", task, "id"))
            throw new Exception("Could not update task.");
    }

    public void DeleteTask(int taskId)
    {
        if (!_repository.DeleteItem("task", taskId))
            throw new Exception("Could not remove task.");
    }

    public IEnumerable<TaskQuery> GetTasksForHive(int hiveId)
    {
        return _repository.GetItemsByParameters<TaskQuery>("task", new { hive_id = hiveId });
    }
}