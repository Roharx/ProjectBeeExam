using System.Collections;
using infrastructure.QueryModels;
using infrastructure.Repositories;

namespace service;

public class BeeService
{
    private readonly BeeRepository _beeRepository;

    public BeeService(BeeRepository beeRepository)
    {
        _beeRepository = beeRepository;
    }

    public IEnumerable<BeeQuery> GetAllBees()
    {
        return _beeRepository.GetAllBees();
    }

    public int CreateBee(string beeName, string beeDescription, string beeComment)
    {
        return _beeRepository.CreateBee(beeName, beeDescription, beeComment);
    }

    public void UpdateBee(BeeQuery bee)
    {
        if (!_beeRepository.UpdateBee(bee))
            throw new Exception("Could not update bee.");
    }
    public void DeleteBee(int beeId)
    {
        if (!_beeRepository.DeleteBee(beeId))
            throw new Exception("Could not remove bee.");
    }

    public BeeQuery GetBeeForHive(int hiveId)
    {
        return _beeRepository.GetBeeForHive(hiveId) ?? throw new Exception("Could not find bee for the hive.");
    }
}