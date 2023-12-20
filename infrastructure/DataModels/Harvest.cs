namespace infrastructure.DataModels;
/// <summary>
/// HoneyAmount: ml
/// BeeswaxAmount: ml
/// </summary>
public class Harvest
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public int HoneyAmount { get; set; }
    public int BeeswaxAmount { get; set; }
}