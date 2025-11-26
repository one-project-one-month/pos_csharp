namespace DotNet8.Pos.App.Models.Report;

public class BestSellingProductModel
{
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public int TotalQuantity { get; set; }
    public decimal TotalAmount { get; set; }
}

public class BestSellingProductDataModel
{
    public List<BestSellingProductModel> Report { get; set; }
}

public class BestSellingProductListResponseModel : ResponseModel
{
    public BestSellingProductDataModel Data { get; set; }
    public PageSettingModel PageSetting { get; set; }
}

