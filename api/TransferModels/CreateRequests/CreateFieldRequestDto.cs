using System.ComponentModel.DataAnnotations;

namespace BeeProject.TransferModels.CreateRequests;

public class CreateFieldRequestDto
{
    [MinLength(3)]public string FieldName { get; set; }
    public string FieldLocation { get; set; }
}