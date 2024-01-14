using Dapper;
using infrastructure.Interfaces;
using Npgsql;

namespace infrastructure.Repositories;

public class RepositoryHandler : IRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public RepositoryHandler(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    /// <summary>
    /// Returns all items from the given table inside the database.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <typeparam name="T">The expected value (eg.: AccountQuery)</typeparam>
    /// <returns>An IEnumerable<T> with the type given to it.</returns>
    public IEnumerable<T> GetAllItems<T>(string tableName)
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
    public IEnumerable<T> GetItemsByParameters<T>(string tableName, object parameters)
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
    /// Returns selected parameters from the given table inside the database that meet the specified conditions.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="columns">Columns to be selected (e.g., "Name, Email").</param>
    /// <param name="parameters">Parameters that the DB requires.</param>
    /// <typeparam name="T">The expected value (e.g., AccountQuery).</typeparam>
    /// <returns></returns>
    public IEnumerable<T> GetSelectedParametersForItems<T>(string tableName, string columns, object parameters)
    {
        var properties = parameters.GetType().GetProperties();
        var whereClause = string.Join(" AND ", properties.Select(prop => $"{prop.Name} = @{prop.Name}"));
        var sql = $"SELECT {columns} FROM {tableName} WHERE {whereClause}";
        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.Query<T>(sql, parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message); // TODO: remove after development
            throw new Exception("An error occurred while fetching selected items.");
        }
    }

    /// <summary>
    /// Return a single item for the parameters. It is a generic code which allows different parameter types and amounts.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="parameters">Parameters that the DB requires</param>
    /// <typeparam name="T">The expected value (eg.: AccountQuery)</typeparam>
    /// <returns></returns>
    public T? GetSingleItemByParameters<T>(string tableName, object parameters)
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
    /// Removes an item from the database. It is a generic code which allows different parameter types.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public bool DeleteItem(string tableName, int itemId)
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
    /// Removes an item from the database. It is a generic code which allows multiple different parameter types.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public bool DeleteItemWithMultipleParams(string tableName, Dictionary<string, object> conditionColumns)
    {
        var conditionClauses = string.Join(" AND ", conditionColumns.Select(cond => $"{cond.Key} = @{cond.Key}"));
        var sql = $"DELETE FROM {tableName} WHERE {conditionClauses}";

        try
        {
            using var conn = _dataSource.OpenConnection();
            conn.Execute(sql, conditionColumns);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);  // TODO: remove after development
            return false;
        }
    }

    /// <summary>
    /// Creates a row inside a table in the database. It is a generic code which allows different parameter types and amounts.
    /// </summary>
    /// <param name="tableName">The name of the table in the DB</param>
    /// <param name="parameters">Parameters that the DB requires</param>
    /// <typeparam name="T">The expected value (eg.: AccountQuery)</typeparam>
    /// <returns></returns>
    public int CreateItem<T>(string tableName, object parameters)
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
    /// Creates a row inside a table in the database. It is a generic code which allows different parameter types and amounts.
    /// </summary>
    /// <param name="tableName">The name of the table in the DB</param>
    /// <param name="parameters">Parameters that the DB requires</param>
    /// <typeparam name="T">The expected value (eg.: AccountQuery)</typeparam>
    /// <returns></returns>
    public bool CreateItemWithoutReturn(string tableName, object parameters)
    {
        var properties = parameters.GetType().GetProperties();
        var columns = string.Join(", ", properties.Select(prop => prop.Name));
        var values = string.Join(", ", properties.Select(prop => $"@{prop.Name}"));

        var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

        try
        {
            using var conn = _dataSource.OpenConnection();
            conn.Execute(sql, parameters);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);//TODO: remove after development
            return false;
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
    public bool UpdateEntity<T>(string tableName, T entity, string conditionColumnName)
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

    /// <summary>
    /// Modifies items in the specified table based on the given parameters and new values.
    /// Usage example:
    /// var conditionColumns = new Dictionary&amp;lt;string, object&amp;gt;
    /// {
    /// { "foreign_key1", 123 },
    /// { "foreign_key2", 456 }
    /// };
    /// 
    /// var modifications = new Dictionary&lt;string, object&gt;
    /// {
    /// { "rank", 5 },
    /// { "status", "active" }
    /// };
    /// repository.ModifyItem("account", conditionColumns, modifications);
    /// </summary>
    /// <param name="tableName">Name of the table inside the DB.</param>
    /// <param name="conditionColumns">A dictionary containing the column names and their corresponding values to be identified by.</param>
    /// <param name="modifications">A dictionary containing the column names and their corresponding new values.</param>
    /// <returns>Returns a boolean value if the modification was successful or not.</returns>
    public bool ModifyItem(string tableName, Dictionary<string, object> conditionColumns, Dictionary<string, object> modifications)
    {
        var conditionClauses = string.Join(" AND ", conditionColumns.Select(cond => $"{cond.Key} = @{cond.Key}"));
        var updateSet = string.Join(", ", modifications.Select(mod => $"{mod.Key} = @{mod.Key}"));
    
        var sql = $"UPDATE {tableName} SET {updateSet} WHERE {conditionClauses}";

        try
        {
            using var conn = _dataSource.OpenConnection();
            
            var parameters = conditionColumns.ToDictionary(conditionColumn => conditionColumn.Key, conditionColumn => conditionColumn.Value);
        
            foreach (var modification in modifications)
            {
                parameters.Add(modification.Key, modification.Value);
            }

            conn.Execute(sql, parameters);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message); // TODO: remove after development
            return false;
        }
    }
}
