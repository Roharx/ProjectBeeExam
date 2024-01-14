using infrastructure.Interfaces;
using infrastructure.QueryModels;

namespace service;

public class HiveService : ServiceBase
{
    public HiveService(IRepository repository) : base(repository)
    { }

    public IEnumerable<HiveQuery> GetAllHives()
    {
        return GetAllItems<HiveQuery>("hive");
    }

    public int CreateHive(int fieldId, string hiveName, string hiveLocation, string placementDate, string lastCheck,
        bool readyToHarvest, string hiveColor, string hiveComment, int beeType)
    {
        var createItemParameters = new
        {
            field_id = fieldId,
            name = hiveName,
            location = hiveLocation,
            placement = placementDate,
            last_check = lastCheck,
            ready = readyToHarvest,
            color = hiveColor,
            comment = hiveComment,
            bee_type = beeType
        };
        
        return CreateItem<int>("hive", createItemParameters);;
    }

    public void UpdateHive(HiveQuery hive)
    {
        UpdateItem("hive", hive);
    }

    public void DeleteHive(int hiveId)
    {
        DeleteItem("hive", hiveId);
    }

    public IEnumerable<HiveQuery> GetHivesForField(int fieldId)
    {
        return GetItemsByParameters<HiveQuery>("hive", new { field_id = fieldId });
    }
}