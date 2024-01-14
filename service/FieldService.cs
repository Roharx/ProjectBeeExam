using infrastructure.Interfaces;
using infrastructure.QueryModels;

namespace service;

public class FieldService
{
    private readonly IRepository _repository;

    public FieldService(IRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<FieldQuery> GetAllFields()
    {
        return _repository.GetAllItems<FieldQuery>("field");
    }

    //TODO: fix later, no usages needed yet
    public IEnumerable<Account_FieldQuery> GetAllAccountFieldConnections()
    {
        return _repository.GetAllItems<Account_FieldQuery>("account_field");
    }

    public int CreateField(string fieldName, string fieldLocation)
    {
        var createItemParameters = new
        {
            name = fieldName,
            location = fieldLocation
        };
        var result = _repository.CreateItem<int>("field", createItemParameters);
        return result != -1 ? result : throw new Exception("Could not create field.");
    }

    public void UpdateField(FieldQuery field)
    {
        if (!_repository.UpdateEntity("field", field, "id"))
            throw new Exception("Could not update field.");
    }

    public void DeleteField(int fieldId)
    {
        if (!_repository.DeleteItem("field", fieldId))
            throw new Exception("Could not remove field.");
    }

    //TODO: placeholder, optimize later
    public IEnumerable<FieldQuery> GetFieldsForAccount(int accountId)
    {
        var fieldIds = _repository.GetItemsByParameters<int>("account_field", new { account_id = accountId }).ToArray();

        return fieldIds.Length != 0
            ? fieldIds.Select(id => _repository.GetSingleItemByParameters<FieldQuery>("field", new { id })!).ToList()
            : Enumerable.Empty<FieldQuery>();
    }
    //TODO: placeholder, optimize later


    public bool ConnectFieldAndAccount(int accountId, int fieldId)
    {
        var createItemParameters = new
        {
            account_id = accountId,
            field_id = fieldId
        };
        return _repository.CreateItemWithoutReturn("account_field", createItemParameters);
    }

    public bool DisconnectFieldAndAccount(int accountId, int fieldId)
    {
        var conditionColumns = new Dictionary<string, object>()
        {
            {"account_id", accountId},
            {"field_id", fieldId}
        };
        
        return _repository.DeleteItemWithMultipleParams("account_field", conditionColumns);
    }
}