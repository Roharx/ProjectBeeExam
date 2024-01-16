using infrastructure.Interfaces;
using infrastructure.QueryModels;

namespace service;

public class FieldService : ServiceBase
{
    //TODO: refactoring, class will be removed later
    public FieldService(IRepository repository) : base(repository)
    { }

    public IEnumerable<FieldQuery> GetAllFields()
    {
        return GetAllItems<FieldQuery>("field");
    }

    public IEnumerable<Account_FieldQuery> GetAllAccountFieldConnections()
    {
        return GetAllItems<Account_FieldQuery>("account_field");
    }

    public int CreateField(string fieldName, string fieldLocation)
    {
        var createItemParameters = new
        {
            name = fieldName,
            location = fieldLocation
        };
        return CreateItem<int>("field", createItemParameters);
    }

    public void UpdateField(FieldQuery field)
    {
        UpdateItem("field", field);
    }

    public void DeleteField(int fieldId)
    {
        DeleteItem("field", fieldId);
    }

    //TODO: placeholder, optimize later
    public IEnumerable<FieldQuery> GetFieldsForAccount(int accountId)
    {
        var fieldIds = GetItemsByParameters<int>("account_field", new { account_id = accountId }).ToArray();

        return fieldIds.Length != 0
            ? fieldIds.Select(id => GetSingleItemByParameters<FieldQuery>("field", new { id })).ToList()
            : Enumerable.Empty<FieldQuery>();
    }

    //TODO:
    public bool ConnectFieldAndAccount(int accountId, int fieldId)
    {
        var createItemParameters = new
        {
            account_id = accountId,
            field_id = fieldId
        };
        return Repository.CreateItemWithoutReturn("account_field", createItemParameters);
    }
    //TODO:
    public bool DisconnectFieldAndAccount(int accountId, int fieldId)
    {
        var conditionColumns = new Dictionary<string, object>()
        {
            {"account_id", accountId},
            {"field_id", fieldId}
        };
        
        return Repository.DeleteItemWithMultipleParams("account_field", conditionColumns);
    }
}