namespace DotNet8.Pos.App.Models.SaleInvoice;

public class SaleInvoiceListResponseModel
{
    public SaleInvoiceDataModel Data { get; set; } = new SaleInvoiceDataModel();
    public PageSettingModel PageSetting { get; set; }
}