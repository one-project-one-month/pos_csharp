namespace DotNet8.Pos.App.Models.SaleInvoice;

public class SaleInvoiceDetailModel
{
    //public int SaleInvoiceDetailId { get; set; }
    //public string? VoucherNo { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Amount { get; set; }
}