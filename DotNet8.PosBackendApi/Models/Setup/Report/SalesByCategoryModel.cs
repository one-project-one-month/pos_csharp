using DotNet8.PosBackendApi.Shared;

namespace DotNet8.PosBackendApi.Models.Setup.Report;

public class SalesByCategoryModel
{
    public string ProductCategoryCode { get; set; }
    public string ProductCategoryName { get; set; }
    public decimal TotalAmount { get; set; }
}

public class SalesByCategoryResponseModel
{
    public List<SalesByCategoryModel> Data { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
    public PageSettingModel PageSetting { get; set; }
}


