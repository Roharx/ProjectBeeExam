using System.Collections;
using infrastructure.QueryModels;
using infrastructure.Repositories;

namespace service;

public class HarvestService
{
    private readonly HarvestRepository _harvestRepository;

    public HarvestService(HarvestRepository harvestRepository)
    {
        _harvestRepository = harvestRepository;
    }

    public IEnumerable<HarvestQuery> GetAllHarvests()
    {
        return _harvestRepository.GetAllHarvests();
    }

    public int CreateHarvest(int hiveId, string harvestTime, int honeyAmount, int beeswaxAmount, string harvestComment)
    {
        var result = _harvestRepository.CreateHarvest(hiveId, harvestTime, honeyAmount, beeswaxAmount, harvestComment);
        return result != -1 ? result : throw new Exception("Could not create harvest.");
    }

    public void UpdateHarvest(HarvestQuery harvest)
    {
        if (!_harvestRepository.UpdateHarvest(harvest))
            throw new Exception("Could not update harvest.");
    }

    public void DeleteHarvest(int harvestId)
    {
        if (!_harvestRepository.DeleteHarvest(harvestId))
            throw new Exception("Could not remove harvest.");
    }

    public IEnumerable<HarvestQuery> GetHarvestForHive(int hiveId)
    {
        return _harvestRepository.GetHarvestsForHive(hiveId);
    }
}