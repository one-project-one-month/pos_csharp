namespace DotNet8.Pos.App.Models.Township;

public class TownshipListResponseModel : ResponseModel
{
    public TownshipDataModel Data { get; set; } = new TownshipDataModel();
    public PageSettingModel PageSetting { get; set; }
}