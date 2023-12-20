using System.ComponentModel.DataAnnotations;

namespace BeeProject.TransferModels.UpdateRequests;

public class UpdateFieldRequestDto
{
    public int FieldId { get; set; }
    [MinLength(3)]public string FieldName { get; set; }
    public string FieldLocation { get; set; }
}