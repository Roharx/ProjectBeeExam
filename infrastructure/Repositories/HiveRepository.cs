using Dapper;
using infrastructure.QueryModels;
using Npgsql;

namespace infrastructure.Repositories;

public class HiveRepository : RepositoryBase
{
    private NpgsqlDataSource _dataSource;

    public HiveRepository(NpgsqlDataSource dataSource) : base(dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<HiveQuery> GetAllHives()
    {
        return GetAllItems<HiveQuery>("hive");
    }

    public int CreateHive(int fieldId, string hiveName, string hiveLocation, string placementDate, string lastCheck,
        bool readyToHarvest, string hiveColor, string hiveComment, int beeType)
    {
        var parameters = new
        {
            field_id = fieldId,
            name = hiveName,
            location = hiveLocation,
            placement = placementDate,
            last_check = lastCheck,
            ready = readyToHarvest,
            color = hiveColor,
            comment = hiveComment,
            bee_type = beeType
        };

        return CreateItem<int>("hive", parameters); //TODO: check if it works, fix if not
    }

    public bool UpdateHive(HiveQuery hive)
    {
        return UpdateEntity("hive", hive, "id");
    }

    public bool DeleteHive(int hiveId)
    {
        return DeleteItem("hive", hiveId);
    }

    public IEnumerable<HiveQuery> GetHivesForFieldId(int fieldId)
    {
        return GetItemsByParameters<HiveQuery>("hive", new { field_id = fieldId });
    }
}