using infrastructure.DataModels.Enums;

namespace infrastructure.DataModels;

public class Ailment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public AilmentSeverity Severity { get; set; }
    public string? Comment { get; set; }
    public bool Solved { get; set; }
}