namespace DotNet8.Pos.App.Models.Report;

public class ReportModel
{
    public DateTime? SaleInvoiceDate { get; set; }
    public decimal TotalAmount { get; set; } = 0;
}