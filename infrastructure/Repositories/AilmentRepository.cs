using Dapper;
using infrastructure.DataModels;
using infrastructure.DataModels.Enums;
using infrastructure.QueryModels;
using Npgsql;

namespace infrastructure.Repositories;

public class AilmentRepository : RepositoryBase
{
    private readonly NpgsqlDataSource _dataSource;

    public AilmentRepository(NpgsqlDataSource dataSource) : base(dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<AilmentQuery> GetAllAilments()
    {
        return GetAllItems<AilmentQuery>("ailment");
    }

    public int CreateAilment(int hiveId, string ailmentName, AilmentSeverity ailmentSeverity, bool ailmentSolved,
        string ailmentComment)
    {
        var parameters = new
        {
            hive_id = hiveId,
            name = ailmentName,
            severity = ailmentSeverity,
            solved = ailmentSolved,
            comment = ailmentComment
        };

        return CreateItem<int>("ailment", parameters); //TODO: check if it works, fix if not
    }

    public bool UpdateAilment(AilmentQuery ailment)
    {
        return UpdateEntity("ailment", ailment, "id");
    }

    public bool DeleteAilment(int ailmentId)
    {
        return DeleteItem("ailment", ailmentId);
    }
    public IEnumerable<AilmentQuery> GetAilmentsForHive(int hiveId)
    {
        return GetItemsByParameters<AilmentQuery>("ailment", new { hive_id = hiveId });
    }

    public IEnumerable<AilmentQuery> GetGlobalAilments()
    {
        const string sql = $@"SELECT * FROM ailment WHERE severity > @treshold"; //TODO: test

        try
        {
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.Query<AilmentQuery>(sql, new { treshold = (int)AilmentSeverity.FieldWide });
            }
        }
        catch (Exception ex)
        {
            //TODO: use globalExceptionHandler later
            return Enumerable.Empty<AilmentQuery>();
        }
    }
    //TODO: Create Ailment, Remove Ailment, Add Ailment to hive
}