namespace DotNet8.Pos.App.Models.ProductCategory;

public class ProductCategoryListResponseModel : ResponseModel
{
    public ProductCategoryDataModel Data { get; set; }
    public PageSettingModel PageSetting { get; set; }
}
