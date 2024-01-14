using infrastructure.Interfaces;
using infrastructure.QueryModels;

namespace service;

public class BeeService
{
    private readonly IRepository _repository;
    public BeeService(IRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<BeeQuery> GetAllBees()
    {
        return _repository.GetAllItems<BeeQuery>("bee");
    }

    public int CreateBee(string beeName, string beeDescription, string beeComment)
    {
        var createItemParameters = new
        {
            name = beeName,
            description = beeDescription,
            comment = beeComment
        };
        
        return _repository.CreateItem<int>("bee", createItemParameters);
    }

    public void UpdateBee(BeeQuery bee)
    {
        if (!_repository.UpdateEntity("bee", bee, "id"))
            throw new Exception("Could not update bee.");
    }
    public void DeleteBee(int beeId)
    {
        if (!_repository.DeleteItem("bee", beeId))
            throw new Exception("Could not remove bee.");
    }

    public BeeQuery GetBeeForHive(int hiveId)
    {
        return _repository.GetSingleItemByParameters<BeeQuery>("bee", new { hive_id = hiveId }) 
               ?? throw new Exception("Could not find bee for the hive.");
    }
}