using infrastructure.Interfaces;
using infrastructure.QueryModels;

namespace service;

public class HoneyService : ServiceBase
{
    public HoneyService(IRepository repository) : base (repository)
    { }

    public IEnumerable<HoneyQuery> GetAllHoney()
    {
        return GetAllItems<HoneyQuery>("honey");
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
        
        return CreateItem<int>("honey", parameters);
    }

    public void UpdateHoney(HoneyQuery honey)
    {
        UpdateItem("honey", honey);
    }

    public void DeleteHoney(int honeyId)
    {
        DeleteItem("honey", honeyId);
    }

    public HoneyQuery GetHoneyForHarvest(int harvestId)
    {
        return GetSingleItemByParameters<HoneyQuery>("honey", new { harvest_id = harvestId })!;

    }
}