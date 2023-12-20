using System.ComponentModel.DataAnnotations;

namespace BeeProject.TransferModels.UpdateRequests;

public class UpdateHiveRequestDto
{
    public int Id { get; set; }
    public int Field_Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$",
        ErrorMessage = "Invalid timestamp format. Use dd-MM-yyyy.")]
    public string Placement { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$",
        ErrorMessage = "Invalid timestamp format. Use yyyy-MM-dd HH:mm:ss.")]
    public string Last_Check { get; set; }

    public bool Ready { get; set; }
    public string Color { get; set; }
    public int Bee_Type { get; set; }
    public string? Comment { get; set; }
}