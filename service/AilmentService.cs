using System.Collections;
using infrastructure.DataModels;
using infrastructure.DataModels.Enums;
using infrastructure.Interfaces;
using infrastructure.QueryModels;
using infrastructure.Repositories;

namespace service;

public class AilmentService
{
    private readonly IRepository _repository;
    public AilmentService(IRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<AilmentQuery> GetAllAilments()
    {
        return _repository.GetAllItems<AilmentQuery>("ailment");
    }

    public int CreateAilment(int hiveId, string ailmentName, AilmentSeverity ailmentSeverity, bool ailmentSolved,
        string ailmentComment)
    {
        var createItemParameters = new
        {
            hive_id = hiveId,
            name = ailmentName,
            severity = (int)ailmentSeverity,
            solved = ailmentSolved,
            comment = ailmentComment
        };
        var result =
            _repository.CreateItem<int>("ailment", createItemParameters);
        return result != -1 ? result : throw new Exception("Couldn't create ailment.");
    }

    public void UpdateAilment(AilmentQuery ailment)
    {
        var result = _repository.UpdateEntity("ailment", ailment, "id");
        if (!result)
            throw new Exception("Couldn't update ailment.");
    }

    public void DeleteAilment(int ailmentId)
    {
        var result = _repository.DeleteItem("ailment", ailmentId);
        if (!result)
            throw new Exception("Couldn't remove ailment.");
    }

    public IEnumerable<AilmentQuery> GetAilmentsForHive(int hiveId)
    {
        return _repository.GetItemsByParameters<AilmentQuery>("ailment", new {hive_id = hiveId});
        
    }
    
    //TODO: handle field-wide ailments on a different layer, it requires hive->field_id

    public IEnumerable<AilmentQuery> GetGlobalAilments()
    {
        return _repository.GetItemsByParameters<AilmentQuery>("ailment", new {severity = (int)AilmentSeverity.SevereInternal});
    }
}