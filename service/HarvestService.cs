using infrastructure.Interfaces;
using infrastructure.QueryModels;

namespace service;

public class HarvestService
{
    private readonly IRepository _repository;

    public HarvestService(IRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<HarvestQuery> GetAllHarvests()
    {
        return _repository.GetAllItems<HarvestQuery>("harvest");
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
        var result = _repository.CreateItem<int>("harvest", createItemParameters);
        return result != -1 ? result : throw new Exception("Could not create harvest.");
    }

    public void UpdateHarvest(HarvestQuery harvest)
    {
        if (!_repository.UpdateEntity("harvest", harvest, "id"))
            throw new Exception("Could not update harvest.");
    }

    public void DeleteHarvest(int harvestId)
    {
        if (!_repository.DeleteItem("harvest", harvestId))
            throw new Exception("Could not remove harvest.");
    }

    public IEnumerable<HarvestQuery> GetHarvestForHive(int hiveId)
    {
        return _repository.GetItemsByParameters<HarvestQuery>("harvest", new { hive_id = hiveId });
    }
}