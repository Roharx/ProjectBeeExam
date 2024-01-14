using infrastructure.Interfaces;
using infrastructure.QueryModels;

namespace service;

public class HiveService
{
    private readonly IRepository _repository;

    public HiveService(IRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<HiveQuery> GetAllHives()
    {
        return _repository.GetAllItems<HiveQuery>("hive");
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
        var result = _repository.CreateItem<int>("hive", createItemParameters);
        return result != -1 ? result : throw new Exception("Could not create hive.");
    }

    public void UpdateHive(HiveQuery hive)
    {
        if (!_repository.UpdateEntity("hive", hive, "id"))
            throw new Exception("Could not update hive.");
    }

    public void DeleteHive(int hiveId)
    {
        if (!_repository.DeleteItem("hive", hiveId))
            throw new Exception("Could not remove hive.");
    }

    public IEnumerable<HiveQuery> GetHivesForField(int fieldId)
    {
        return _repository.GetItemsByParameters<HiveQuery>("hive", new { field_id = fieldId });
    }
}