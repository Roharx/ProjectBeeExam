using System.ComponentModel.DataAnnotations;

namespace BeeProject.TransferModels.CreateRequests;

public class CreateHarvestRequestDto
{
    public int HiveId { get; set; }
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$",
        ErrorMessage = "Invalid timestamp format. Use dd-MM-yyyy HH:mm:ss.")]
    public string Time { get; set; }
    public int HoneyAmount { get; set; }
    public int BeeswaxAmount { get; set; }
    public string Comment { get; set; }
}