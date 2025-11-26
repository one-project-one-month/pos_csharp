using DotNet8.PosBackendApi.Shared;

namespace DotNet8.PosBackendApi.Models.Setup.Report;

public class BestSellingProductModel
{
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public int TotalQuantity { get; set; }
    public decimal TotalAmount { get; set; }
}

public class BestSellingProductResponseModel
{
    public List<BestSellingProductModel> Data { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
    public PageSettingModel PageSetting { get; set; }
}


