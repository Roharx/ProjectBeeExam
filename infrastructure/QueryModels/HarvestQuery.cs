namespace infrastructure.QueryModels;

public class HarvestQuery
{
    public int Id { get; set; }
    public int Hive_Id { get; set; }
    public string Time { get; set; }
    public int Honey_Amount { get; set; }
    public int Beeswax_Amount { get; set; }
    public string? Comment { get; set; }
}