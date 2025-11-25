namespace DotNet8.Pos.App.Models.Shop;

public class ShopListResponseModel : ResponseModel
{
    public ShopDataModel Data { get; set; }
    public PageSettingModel PageSetting { get; set; }
}