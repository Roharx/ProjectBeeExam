using infrastructure.QueryModels;
using infrastructure.Repositories;

namespace service;

public class FieldService
{
    private readonly FieldRepository _fieldRepository;

    public FieldService(FieldRepository fieldRepository)
    {
        _fieldRepository = fieldRepository;
    }

    public IEnumerable<FieldQuery> GetAllFields()
    {
        return _fieldRepository.GetAllFields();
    }

    public IEnumerable<Account_FieldQuery> GetAllAccountFieldConnections()
    {
        return _fieldRepository.GetAllAccountFieldConnections();
    }
    public int CreateField(string fieldName, string fieldLocation)
    {
        var result = _fieldRepository.CreateField(fieldName, fieldLocation);
        return result != -1 ? result : throw new Exception("Could not create field.");
    }

    public void UpdateField(FieldQuery field)
    {
        if (!_fieldRepository.UpdateField(field))
            throw new Exception("Could not update field.");
    }

    public void DeleteField(int fieldId)
    {
        if (!_fieldRepository.DeleteField(fieldId))
            throw new Exception("Could not remove field.");
    }

    //TODO: placeholder, optimize later
    public IEnumerable<FieldQuery> GetFieldsForAccount(int accountId)
    {
        var fieldIds = _fieldRepository.GetFieldIdsForAccount(accountId).ToArray();

        return fieldIds.Length != 0
            ? fieldIds.Select(id => _fieldRepository.GetFieldById(id)!).ToList()
            : Enumerable.Empty<FieldQuery>();
    }
    //TODO: placeholder, optimize later
    

    public bool ConnectFieldAndAccount(int accountId, int fieldId)
    {
        return _fieldRepository.ConnectFieldAndAccount(accountId, fieldId);
    }

    public bool DisconnectFieldAndAccount(int accountId, int fieldId)
    {
        return _fieldRepository.DisconnectFieldAndAccount(accountId, fieldId);
    }
}