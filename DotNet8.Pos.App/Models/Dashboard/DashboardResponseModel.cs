namespace DotNet8.Pos.App.Models.Dashboard;

public class DashboardResponseModel : ResponseModel
{
    public string Message { get; set; }
    public string Token { get; set; }
    public bool IsSuccess { get; set; }
    public Data Data { get; set; } = new Data();
}

public class Data
{
    public Dashboard Dashboard { get; set; } = new Dashboard();
}

public class Dashboard
{
    public List<DailyDashboardModel> DailyData { get; set; } = new List<DailyDashboardModel>();
    public List<WeeklyDashboardModel> WeeklyData { get; set; } = new List<WeeklyDashboardModel>();
    public List<MonthlyDashboardModel> MonthlyData { get; set; } = new List<MonthlyDashboardModel>();
    public List<YearlyDashboardModel> YearlyData { get; set; } = new List<YearlyDashboardModel>();
    public List<BestSellerProduct> BestSellerProduct { get; set; } = new List<BestSellerProduct>();
}

public class DailyDashboardModel
{
    public DateTime SaleInvoiceDate { get; set; }
    public decimal Amount { get; set; }
}

public class WeeklyDashboardModel
{
    public DateTime SaleInvoiceDate { get; set; }
    public decimal Amount { get; set; }
}

public class MonthlyDashboardModel
{
    public DateTime SaleInvoiceDate { get; set; }
    public decimal Amount { get; set; }
}

public class YearlyDashboardModel
{
    public int Year { get; set; }
    public decimal Amount { get; set; }
}

public class BestSellerProduct
{
    public string ProductName { get; set; }
    public int TotalQty { get; set; }
}
