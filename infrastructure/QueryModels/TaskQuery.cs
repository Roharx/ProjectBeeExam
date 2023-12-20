namespace infrastructure.QueryModels;

public class TaskQuery
{
    public int Id { get; set; }
    public int Hive_Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool Done { get; set; }
}