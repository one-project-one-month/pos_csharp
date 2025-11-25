namespace DotNet8.Pos.App.Models.SaleInvoice;

public class SaleInvoiceModel
{
    public int SaleInvoiceId { get; set; }
    public DateTime SaleInvoiceDateTime { get; set; }
    public string? VoucherNo { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public string StaffCode { get; set; }
    public decimal Tax { get; set; }
    public string? PaymentType { get; set; }
    public string? CustomerAccountNo { get; set; }
    public decimal? PaymentAmount { get; set; } = 0;
    public decimal? ReceiveAmount { get; set; } = 0;
    public decimal? Change { get { return ReceiveAmount - PaymentAmount; } }
    public string? CustomerCode { get; set; }
    public List<SaleInvoiceDetailModel>? SaleInvoiceDetails { get; set; } = new List<SaleInvoiceDetailModel>();
}

public class SaleInvoiceItemModel
{
    public SaleInvoiceModel SaleInvoice { get; set; } = new SaleInvoiceModel();
}