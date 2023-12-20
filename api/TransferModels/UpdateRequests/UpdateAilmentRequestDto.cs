using System.ComponentModel.DataAnnotations;

namespace BeeProject.TransferModels.UpdateRequests;

public class UpdateAilmentRequestDto
{
    public int Id { get; set; }
    public int HiveId { get; set; }
    [MinLength(3)] public string Name { get; set; }

    [Range(0, 5, ErrorMessage = "Must be a number between {1} and {2}.")]
    public int Severity { get; set; }

    public string? Comment { get; set; }
    public bool Solved { get; set; }
}