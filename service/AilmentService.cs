using System.Collections;
using infrastructure.DataModels;
using infrastructure.DataModels.Enums;
using infrastructure.QueryModels;
using infrastructure.Repositories;

namespace service;

public class AilmentService
{
    private readonly AilmentRepository _ailmentRepository;

    public AilmentService(AilmentRepository ailmentRepository)
    {
        _ailmentRepository = ailmentRepository;
    }

    public IEnumerable<AilmentQuery> GetAllAilments()
    {
        return _ailmentRepository.GetAllAilments();
    }

    public int CreateAilment(int hiveId, string ailmentName, AilmentSeverity ailmentSeverity, bool ailmentSolved,
        string ailmentComment)
    {
        var result =
            _ailmentRepository.CreateAilment(hiveId, ailmentName, ailmentSeverity, ailmentSolved, ailmentComment);
        return result != -1 ? result : throw new Exception("Couldn't create ailment.");
    }

    public void UpdateAilment(AilmentQuery ailment)
    {
        var result = _ailmentRepository.UpdateAilment(ailment);
        if (!result)
            throw new Exception("Couldn't update ailment.");
    }

    public void DeleteAilment(int ailmentId)
    {
        var result = _ailmentRepository.DeleteAilment(ailmentId);
        if (!result)
            throw new Exception("Couldn't remove ailment.");
    }

    public IEnumerable<AilmentQuery> GetAilmentsForHive(int hiveId)
    {
        return _ailmentRepository.GetAilmentsForHive(hiveId);
        //TODO: check if there should be exception or just return null as it is
    }
    
    //TODO: handle field-wide ailments on a different layer, it requires hive->field_id

    public IEnumerable<AilmentQuery> GetGlobalAilments()
    {
        return _ailmentRepository.GetGlobalAilments();
    }
}