namespace DotNet8.Pos.App.Models.Report;

public class ReportRequestModel
{
    public DateTime? SaleInvoiceDate { get; set; }
    public decimal TotalAmount { get; set; } = 0;
}