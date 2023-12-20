using Dapper;
using infrastructure.DataModels;
using infrastructure.QueryModels;
using Npgsql;

namespace infrastructure.Repositories;

public class HoneyRepository : RepositoryBase
{
    private NpgsqlDataSource _dataSource;

    public HoneyRepository(NpgsqlDataSource dataSource) : base(dataSource)
    {
        _dataSource = dataSource;
    }

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

    public bool UpdateHoney(HoneyQuery honey)
    {
        return UpdateEntity("honey", honey, "id");
    }

    public bool DeleteHoney(int honeyId)
    {
        return DeleteItem("honey", honeyId);
    }

    public HoneyQuery? GetHoneyForHarvest(int harvestId)
    {
        return GetSingleItemByParameters<HoneyQuery>("honey", new { harvest_id = harvestId });
    }
}