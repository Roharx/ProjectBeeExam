using Dapper;
using Npgsql;

namespace infrastructure.Repositories;

public class RepositoryBase
{
    private readonly NpgsqlDataSource _dataSource;

    protected RepositoryBase(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    /// <summary>
    /// Returns all items from the given table inside the database.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <typeparam name="T">The expected value (eg.: AccountQuery)</typeparam>
    /// <returns>An IEnumerable<T> with the type given to it.</returns>
    protected IEnumerable<T> GetAllItems<T>(string tableName)
    {
        var sql = $"SELECT * FROM {tableName}";

        try
        {
            using var conn = _dataSource.OpenConnection();
            var result = conn.Query<T>(sql);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);//TODO: remove after development
            throw new Exception("crap");
        }
    }

    /// <summary>
    /// Returns all items from the given table inside the database that has the given parameters.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="parameters">Parameters that the DB requires</param>
    /// <typeparam name="T">The expected value (eg.: AccountQuery)</typeparam>
    /// <returns></returns>
    protected IEnumerable<T> GetItemsByParameters<T>(string tableName, object parameters)
    {
        var properties = parameters.GetType().GetProperties();
        var whereClause = string.Join(" AND ", properties.Select(prop => $"{prop.Name} = @{prop.Name}"));
        var sql = $"SELECT * FROM {tableName} WHERE {whereClause}";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.Query<T>(sql, parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);//TODO: remove after development
            throw new Exception("crap");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="parameters">Parameters that the DB requires</param>
    /// <typeparam name="T">The expected value (eg.: AccountQuery)</typeparam>
    /// <returns></returns>
    protected T? GetSingleItemByParameters<T>(string tableName, object parameters)
    {
        var properties = parameters.GetType().GetProperties();
        var whereClause = string.Join(" AND ", properties.Select(prop => $"{prop.Name} = @{prop.Name}"));
        var sql = $"SELECT * FROM {tableName} WHERE {whereClause}";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.QueryFirstOrDefault<T>(sql, parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);//TODO: remove after development
            throw new Exception("crap");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="itemId"></param>
    /// <returns></returns>
    protected bool DeleteItem(string tableName, int itemId)
    {
        var sql = $"DELETE FROM {tableName} WHERE id=@id";
        try
        {
            using var conn = _dataSource.OpenConnection();
            conn.Execute(sql, new { id = itemId });
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);//TODO: remove after development
            return false;
        }
    }

    /// <summary>
    /// This method is used to create a row inside a table in the database. It is a generic code which allows different parameter types and amounts.
    /// </summary>
    /// <param name="tableName">The name of the table in the DB</param>
    /// <param name="parameters">Parameters that the DB requires</param>
    /// <typeparam name="T">The expected value (eg.: AccountQuery)</typeparam>
    /// <returns></returns>
    protected int CreateItem<T>(string tableName, object parameters)
    {
        var properties = parameters.GetType().GetProperties();
        var columns = string.Join(", ", properties.Select(prop => prop.Name));
        var values = string.Join(", ", properties.Select(prop => $"@{prop.Name}"));

        var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values}) RETURNING id";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.ExecuteScalar<int>(sql, parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);//TODO: remove after development
            return -1;
        }
    }

    /// <summary>
    /// Updates a row inside a table in the database. It is a generic code which allows different parameter types and amounts.
    /// </summary>
    /// <param name="tableName">Name of the table inside the DB.</param>
    /// <param name="entity">The entity that has the parameters (eg.: Account account).</param>
    /// <param name="conditionColumnName">The name of the column which the row can be found by inside the DB.</param>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <returns>Returns a boolean value if the update was successful or not.</returns>
    protected bool UpdateEntity<T>(string tableName, T entity, string conditionColumnName)
    {
        var properties = typeof(T).GetProperties();

        // Exclude the property used in the WHERE clause from the update set
        var updateSet = string.Join(", ", properties.Where(prop => prop.Name != conditionColumnName)
            .Select(prop => $"{prop.Name} = @{prop.Name}"));

        var sql = $"UPDATE {tableName} SET {updateSet} WHERE {conditionColumnName} = @{conditionColumnName}";

        try
        {
            using var conn = _dataSource.OpenConnection();
            conn.Execute(sql, entity);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);//TODO: remove after development
            return false;
        }
    }
}