using infrastructure.Interfaces;
using infrastructure.QueryModels;

namespace service;

public class BeeService : ServiceBase
{
    public BeeService(IRepository repository) : base(repository)
    { }

    public IEnumerable<BeeQuery> GetAllBees()
    {
        return GetAllItems<BeeQuery>("bee");
    }

    public int CreateBee(string beeName, string beeDescription, string beeComment)
    {
        var createItemParameters = new
        {
            name = beeName,
            description = beeDescription,
            comment = beeComment
        };
        
        return CreateItem<int>("bee", createItemParameters);
    }

    public void UpdateBee(BeeQuery bee)
    {
        UpdateItem("bee", bee);
    }
    public void DeleteBee(int beeId)
    {
        DeleteItem("bee", beeId);
    }

    public BeeQuery GetBeeForHive(int hiveId)
    {
        return GetSingleItemByParameters<BeeQuery>("bee", new { hive_id = hiveId })!;

    }
}