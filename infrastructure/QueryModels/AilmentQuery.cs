using infrastructure.DataModels;

namespace infrastructure.QueryModels;

public class AilmentQuery
{
    public int Id { get; set; }
    public int Hive_Id { get; set; }
    public string Name { get; set; }
    public int Severity { get; set; }
    public string? Comment { get; set; }
    public bool Solved { get; set; }
}