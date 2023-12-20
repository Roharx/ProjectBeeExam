namespace infrastructure.QueryModels;

public class HiveQuery
{
    public int Id { get; set; }
    public int Field_Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string Placement { get; set; }//timestamp
    public string Last_Check { get; set; }//timestamp
    public bool Ready { get; set; }
    public string Color { get; set; }
    public int Bee_Type { get; set; }
    public string? Comment { get; set; }
}
//TODO: comments are for dev only, remove at release