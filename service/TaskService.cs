using infrastructure.Interfaces;
using infrastructure.QueryModels;

namespace service;

public class TaskService : ServiceBase
{
    //TODO: refactoring, class will be removed later
    public TaskService(IRepository repository) : base (repository)
    { }

    public IEnumerable<TaskQuery> GetAllTasks()
    {
        return GetAllItems<TaskQuery>("task");
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
        return CreateItem<int>("task", parameters);
    }

    public void UpdateTask(TaskQuery task)
    {
        UpdateItem("task", task);
    }

    public void DeleteTask(int taskId)
    {
        DeleteItem("task", taskId);
    }

    public IEnumerable<TaskQuery> GetTasksForHive(int hiveId)
    {
        return GetItemsByParameters<TaskQuery>("task", new { hive_id = hiveId });
    }
}