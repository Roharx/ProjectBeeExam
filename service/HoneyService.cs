using infrastructure.Interfaces;
using infrastructure.QueryModels;

namespace service;

public class HoneyService
{
    private readonly IRepository _repository;
    public HoneyService(IRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<HoneyQuery> GetAllHoney()
    {
        return _repository.GetAllItems<HoneyQuery>("honey");
    }

    public int CreateHoney(int harvestId, string honeyName, bool honeyLiquidity, string honeyFlowers,
        float honeyMoisture)
    {
        var parameters = new
        {
            harvest_id = harvestId,
            name = honeyName,
            liquid = honeyLiquidity,
            flowers = honeyFlowers,
            moisture = honeyMoisture
        };
        var result = _repository.CreateItem<int>("honey", parameters);
        return result != -1 ? result : throw new Exception("Could not create honey.");
    }

    public void UpdateHoney(HoneyQuery honey)
    {
        if (!_repository.UpdateEntity("honey", honey, "id"))
            throw new Exception("Could not update honey.");
    }

    public void DeleteHoney(int honeyId)
    {
        if (!_repository.DeleteItem("honey", honeyId))
            throw new Exception("Could not remove honey.");
    }

    public HoneyQuery GetHoneyForHarvest(int harvestId)
    {
        return _repository.GetSingleItemByParameters<HoneyQuery>("honey", new { harvest_id = harvestId }) ??
               throw new Exception("Could not find honey for harvest.");
    }
}