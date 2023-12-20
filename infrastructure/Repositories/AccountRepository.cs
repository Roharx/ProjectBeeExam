using Dapper;
using infrastructure.QueryModels;
using Npgsql;

namespace infrastructure.Repositories;

public class AccountRepository : RepositoryBase
{
    private readonly NpgsqlDataSource _dataSource;

    public AccountRepository(NpgsqlDataSource dataSource) : base(dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<AccountSafeQuery> GetAllAccounts()
    {
        //TODO: remove GetAllItems
        //return GetAllItems<AccountQuery>("account"); dev only
        const string sql = "SELECT id, email, name, rank FROM account";

        using var conn = _dataSource.OpenConnection();
        return conn.Query<AccountSafeQuery>(sql);
    }

    public IEnumerable<int> GetAccountsForField(int fieldId) //later on can generalize in RepositoryBase if have time
    {
        const string sql = $"SELECT account_id FROM account_field WHERE field_id=@fieldId";
        try
        {
            using var conn = _dataSource.OpenConnection();

            return conn.Query<int>(sql, new { fieldId });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message); //TODO: remove after development
            throw new Exception("No accounts found for field id."); //this shouldn't happen unless there's a major bug
        }
    }

    public AccountSafeQuery GetAccountById(int id)
    {
        return GetSingleItemByParameters<AccountSafeQuery>("account", new { id });
    }

    public IEnumerable<AccountSafeQuery> GetAccountNamesForRank(int rank)
    {
        const string sql = "SELECT id, email, name, rank FROM account WHERE rank=@rank";
        using var conn = _dataSource.OpenConnection();
        return conn.Query<AccountSafeQuery>(sql, new { rank});
    }

    public int CreateAccount(string accountName, string accountEmail, string accountPassword, int accountRank)
    {
        var parameters = new
        {
            name = accountName,
            email = accountEmail,
            password = accountPassword,
            rank = accountRank
        };

        return CreateItem<int>("account", parameters); //TODO: check if it works, fix if not
    }

    public bool UpdateAccount(AccountQuery account)
    {
        return UpdateEntity("account", account, "id");
    }

    public bool DeleteAccount(int accountId)
    {
        return DeleteItem("account", accountId);
    }

    public bool ModifyAccountRank(int accountId, int rank)
    {
        var sql = "UPDATE account SET rank=@rank WHERE id=@accountId";

        try
        {
            using var conn = _dataSource.OpenConnection();
            conn.Execute(sql, new {rank, accountId});
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);//TODO: remove after development
            return false;
        }
    }

    public AccountQuery? GetAccountByName(string name)
    {
        return GetSingleItemByParameters<AccountQuery>("account", new { name });
    }
}