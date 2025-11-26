namespace DotNet8.Pos.App.Models.Report;

public class SalesByCategoryModel
{
    public string ProductCategoryCode { get; set; }
    public string ProductCategoryName { get; set; }
    public decimal TotalAmount { get; set; }
}

public class SalesByCategoryDataModel
{
    public List<SalesByCategoryModel> Report { get; set; }
}

public class SalesByCategoryListResponseModel : ResponseModel
{
    public SalesByCategoryDataModel Data { get; set; }
    public PageSettingModel PageSetting { get; set; }
}

