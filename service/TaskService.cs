using infrastructure.QueryModels;
using infrastructure.Repositories;

namespace service;

public class TaskService
{
    private readonly TaskRepository _taskRepository;

    public TaskService(TaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public IEnumerable<TaskQuery> GetAllTasks()
    {
        return _taskRepository.GetAllTasks();
    }

    public int CreateTask(int hiveId, string taskName, string taskDescription, bool taskDone)
    {
        var result = _taskRepository.CreateTask(hiveId, taskName, taskDescription, taskDone);
        return result != -1 ? result : throw new Exception("Could not create task.");
    }

    public void UpdateTask(TaskQuery task)
    {
        if (!_taskRepository.UpdateTask(task))
            throw new Exception("Could not update task.");
    }

    public void DeleteTask(int taskId)
    {
        if (!_taskRepository.DeleteTask(taskId))
            throw new Exception("Could not remove task.");
    }

    public IEnumerable<TaskQuery> GetTasksForHive(int hiveId)
    {
        return _taskRepository.GetTasksForHive(hiveId);
    }
}