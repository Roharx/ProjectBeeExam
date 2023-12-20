using infrastructure.QueryModels;
using infrastructure.Repositories;

namespace service;

public class HiveService
{
    private readonly HiveRepository _hiveRepository;

    public HiveService(HiveRepository hiveRepository)
    {
        _hiveRepository = hiveRepository;
    }

    public IEnumerable<HiveQuery> GetAllHives()
    {
        return _hiveRepository.GetAllHives();
    }

    public int CreateHive(int fieldId, string hiveName, string hiveLocation, string placementDate, string lastCheck,
        bool readyToHarvest, string hiveColor, string hiveComment, int beeType)
    {
        var result = _hiveRepository.CreateHive(fieldId, hiveName, hiveLocation, placementDate, lastCheck,
            readyToHarvest,
            hiveColor, hiveComment, beeType);
        return result != -1 ? result : throw new Exception("Could not create hive.");
    }

    public void UpdateHive(HiveQuery hive)
    {
        if (!_hiveRepository.UpdateHive(hive))
            throw new Exception("Could not update hive.");
    }

    public void DeleteHive(int hiveId)
    {
        if (!_hiveRepository.DeleteHive(hiveId))
            throw new Exception("Could not remove hive.");
    }

    public IEnumerable<HiveQuery> GetHivesForField(int fieldId)
    {
        return _hiveRepository.GetHivesForFieldId(fieldId);
    }
}