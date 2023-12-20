using infrastructure.QueryModels;
using infrastructure.Repositories;

namespace service;

public class HoneyService
{
    private readonly HoneyRepository _honeyRepository;

    public HoneyService(HoneyRepository honeyRepository)
    {
        _honeyRepository = honeyRepository;
    }

    public IEnumerable<HoneyQuery> GetAllHoney()
    {
        return _honeyRepository.GetAllHoney();
    }

    public int CreateHoney(int harvestId, string honeyName, bool honeyLiquidity, string honeyFlowers,
        float honeyMoisture)
    {
        var result = _honeyRepository.CreateHoney(harvestId, honeyName, honeyLiquidity, honeyFlowers, honeyMoisture);
        return result != -1 ? result : throw new Exception("Could not create honey.");
    }

    public void UpdateHoney(HoneyQuery honey)
    {
        if (!_honeyRepository.UpdateHoney(honey))
            throw new Exception("Could not update honey.");
    }

    public void DeleteHoney(int honeyId)
    {
        if (!_honeyRepository.DeleteHoney(honeyId))
            throw new Exception("Could not remove honey.");
    }

    public HoneyQuery GetHoneyForHarvest(int harvestId)
    {
        return _honeyRepository.GetHoneyForHarvest(harvestId) ??
               throw new Exception("Could not find honey for harvest.");
    }
}