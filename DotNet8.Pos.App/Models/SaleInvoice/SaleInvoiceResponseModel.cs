namespace DotNet8.Pos.App.Models.SaleInvoice;

public class SaleInvoiceResponseModel : ResponseModel
{
    public SaleInvoiceItemModel Data { get; set; } = new SaleInvoiceItemModel();
}