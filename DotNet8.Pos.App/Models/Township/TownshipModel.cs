namespace DotNet8.Pos.App.Models.Township;

public class TownshipModel
{
    public int TownshipId { get; set; }
    public string? TownshipCode { get; set; }
    public string? TownshipName { get; set; }
    public string? StateCode { get; set; }
    public string? StateName { get; set;}
}

public class TownshipItemModel
{
    public TownshipModel Township { get; set; } = new TownshipModel();
}