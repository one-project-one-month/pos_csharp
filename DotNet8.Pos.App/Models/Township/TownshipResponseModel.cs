namespace DotNet8.Pos.App.Models.Township;

public class TownshipResponseModel : ResponseModel
{
    public TownshipItemModel Data { get; set; } = new TownshipItemModel();
}