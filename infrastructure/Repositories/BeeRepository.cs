using infrastructure.QueryModels;
using Npgsql;

namespace infrastructure.Repositories;

public class BeeRepository : RepositoryBase
{
    private readonly NpgsqlDataSource _dataSource;

    public BeeRepository(NpgsqlDataSource dataSource) : base(dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<BeeQuery> GetAllBees()
    {
        return GetAllItems<BeeQuery>("bee");
    }

    public int CreateBee(string beeName, string beeDescription, string beeComment)
    {
        var parameters = new
        {
            name = beeName,
            description = beeDescription,
            comment = beeComment
        };

        return CreateItem<int>("bee", parameters); //TODO: check if it works, fix if not
    }

    public bool UpdateBee(BeeQuery bee)
    {
        return UpdateEntity("bee", bee, "id");
    }

    public bool DeleteBee(int beeId)
    {
        return DeleteItem("bee", beeId);
    }

    public BeeQuery? GetBeeForHive(int hiveId)
    {
        return GetSingleItemByParameters<BeeQuery>("bee", new { hive_id = hiveId });
    }
}