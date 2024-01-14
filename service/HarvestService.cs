using infrastructure.Interfaces;
using infrastructure.QueryModels;

namespace service;

public class HarvestService : ServiceBase
{
    public HarvestService(IRepository repository) : base(repository)
    { }

    public IEnumerable<HarvestQuery> GetAllHarvests()
    {
        return GetAllItems<HarvestQuery>("harvest");
    }

    public int CreateHarvest(int hiveId, string harvestTime, int honeyAmount, int beeswaxAmount, string harvestComment)
    {
        var createItemParameters = new
        {
            hive_id = hiveId,
            time = harvestTime,
            honey_amount = honeyAmount,
            beeswax_amount = beeswaxAmount,
            comment = harvestComment
        };
        
        return CreateItem<int>("harvest", createItemParameters);
    }

    public void UpdateHarvest(HarvestQuery harvest)
    {
        UpdateItem("harvest", harvest);
    }

    public void DeleteHarvest(int harvestId)
    {
        DeleteItem("harvest", harvestId);
    }

    public IEnumerable<HarvestQuery> GetHarvestForHive(int hiveId)
    {
        return GetItemsByParameters<HarvestQuery>("harvest", new { hive_id = hiveId });
    }
}