using infrastructure.DataModels.Enums;
using infrastructure.Interfaces;
using infrastructure.QueryModels;

namespace service;

public class AilmentService : ServiceBase
{
    public AilmentService(IRepository repository) : base(repository)
    { }

    public IEnumerable<AilmentQuery> GetAllAilments()
    {
        return GetAllItems<AilmentQuery>("ailment");
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
        
        return CreateItem<int>("ailment", createItemParameters);
    }

    public void UpdateAilment(AilmentQuery ailment)
    {
        UpdateItem("ailment", ailment);
    }

    public void DeleteAilment(int ailmentId)
    {
        DeleteItem("ailment", ailmentId);
    }

    public IEnumerable<AilmentQuery> GetAilmentsForHive(int hiveId)
    {
        return GetItemsByParameters<AilmentQuery>("ailment", new {hive_id = hiveId});
        
    }
    
    //TODO: field-wide ailments

    public IEnumerable<AilmentQuery> GetGlobalAilments()
    {
        return GetItemsByParameters<AilmentQuery>("ailment", new {severity = (int)AilmentSeverity.SevereInternal});
    }
}