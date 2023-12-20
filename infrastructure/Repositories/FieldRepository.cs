using Dapper;
using infrastructure.QueryModels;
using Npgsql;

namespace infrastructure.Repositories;

public class FieldRepository : RepositoryBase
{
    private readonly NpgsqlDataSource _dataSource;

    public FieldRepository(NpgsqlDataSource dataSource) : base(dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<FieldQuery> GetAllFields()
    {
        return GetAllItems<FieldQuery>("field");
    }
    public int CreateField(string fieldName, string fieldLocation)
    {
        var parameters = new
        {
            name = fieldName,
            location = fieldLocation
        };

        return CreateItem<int>("field", parameters); //TODO: check if it works, fix if not
    }

    public IEnumerable<Account_FieldQuery> GetAllAccountFieldConnections()
    {
        return GetAllItems<Account_FieldQuery>("account_field");
    }
    public bool UpdateField(FieldQuery field)
    {
        return UpdateEntity("field", field, "id");
    }

    public bool DeleteField(int fieldId)
    {
        return DeleteItem("field", fieldId);
    }

    //TODO: change location later probably
    
    
    /// <summary>
    /// Input: Account id, Output: array of Field ids
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>Returns an integer array that contains the ids for all the fields that are connected to the account id.</returns>
    public IEnumerable<int> GetFieldIdsForAccount(int accountId)
    {
        const string sql = $"SELECT field_id FROM account_field WHERE account_id=@accountId";
        try
        {
            using var conn = _dataSource.OpenConnection();
            
            return conn.Query<int>(sql, new { accountId });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);//TODO: remove after development
            throw new Exception("No fields found for account id.");
        }
    }
    public bool ConnectFieldAndAccount(int accountId, int fieldId)
    {
        const string sql = $"INSERT INTO account_field (account_id, field_id) VALUES (@accountId, @fieldId)";
        try
        {
            using var conn = _dataSource.OpenConnection();
            conn.Execute(sql, new { accountId, fieldId });
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);//TODO: remove after development
            return false;
        }
    }

    public bool DisconnectFieldAndAccount(int accountId, int fieldId)
    {
        const string sql = $"DELETE FROM account_field WHERE account_id=@accountId AND field_id=@fieldId";
        try
        {
            using var conn = _dataSource.OpenConnection();
            conn.Execute(sql, new { accountId, fieldId });
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);//TODO: remove after development
            return false;
        }
    }

    public FieldQuery? GetFieldById(int fieldId)
    {
        var parameters = new { id = fieldId };
        return GetSingleItemByParameters<FieldQuery>("field", parameters);
    }
}