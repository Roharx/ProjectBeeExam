using infrastructure.QueryModels;
using Npgsql;

namespace infrastructure.Repositories;

public class HarvestRepository : RepositoryBase
{
    private readonly NpgsqlDataSource _dataSource;

    public HarvestRepository(NpgsqlDataSource dataSource) : base(dataSource)
    {
        _dataSource = dataSource;
    }
    public IEnumerable<HarvestQuery> GetAllHarvests()
    {
        return GetAllItems<HarvestQuery>("harvest");
    }

    public int CreateHarvest(int hiveId, string harvestTime, int honeyAmount, int beeswaxAmount, string harvestComment)
    {
        var parameters = new
        {
            hive_id = hiveId,
            time = harvestTime,
            honey_amount = honeyAmount,
            beeswax_amount = beeswaxAmount,
            comment = harvestComment
        };

        return CreateItem<int>("harvest", parameters);//TODO: check if it works, fix if not
    }

    public bool UpdateHarvest(HarvestQuery harvest)
    {
        return UpdateEntity("harvest", harvest, "id");
    }

    public bool DeleteHarvest(int harvestId)
    {
        return DeleteItem("harvest", harvestId);
    }

    public IEnumerable<HarvestQuery> GetHarvestsForHive(int hiveId)
    {
        return GetItemsByParameters<HarvestQuery>("harvest", new { hive_id = hiveId });
    }
}