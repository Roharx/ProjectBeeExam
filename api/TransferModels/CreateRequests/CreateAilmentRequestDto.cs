using System.ComponentModel.DataAnnotations;

namespace BeeProject.TransferModels.CreateRequests;

public class CreateAilmentRequestDto
{
    public int Hive_Id { get; set; }
    [MinLength(3)] public string Name { get; set; }

    [Range(0, 5, ErrorMessage = "Must be a number between {1} and {2}.")]
    public int Severity { get; set; }

    public string? Comment { get; set; }
    public bool Solved { get; set; }
}