using DotNet8.PosBackendApi.Models.Setup.Report;

namespace DotNet8.PosBackendApi.Features.Report;

public class BL_Report
{
    private readonly DL_Report _report;

    public BL_Report(DL_Report report) => _report = report;

    public async Task<MonthlyReportResponseModel> DailyReport(int dateDay, int dateMonth, int dateYear, int pageNo,int pageSize)
    {
        var response = await _report.DailyReport(dateDay, dateMonth, dateYear, pageNo, pageSize);
        return response;
    }

    //public async Task<MonthlyReportResponseModel> DailyReportV1(SaleDailyReportRequestModel requestModel)
    //{
    //    SaleDailyReportRequestModelCheck(requestModel);
    //    var response = await _report.DailyReportV1(requestModel);
    //    return response;
    //}

    public async Task<MonthlyReportResponseModel> MonthlyReport(int month, int year, int pageNo, int pageSize)
    {
        var response = await _report.MonthlyReport(month, year, pageNo, pageSize);
        return response;
    }

    public async Task<MonthlyReportResponseModel> YearlyReport(int year, int pageNo, int pageSize)
    {
        var response = await _report.YearlyReport(year, pageNo, pageSize);
        return response;
    }

    public async Task<ReportResponseModel> DailyReport(DateTime fromDate, DateTime toDate, int pageNo, int pageSize)
    {
        var response = await _report.DailyReport(fromDate, toDate, pageNo, pageSize);
        return response;
    }

    public async Task<ReportResponseModel> MonthlyReport(DateTime fromDate, DateTime toDate, int pageNo, int pageSize)
    {
        var response = await _report.MonthlyReport(fromDate, toDate, pageNo, pageSize);
        return response;
    }

    public async Task<MonthlyReportResponseModel> YearlyReport(DateTime fromDate, DateTime toDate, int pageNo, int pageSize)
    {
        var response = await _report.YearlyReport(fromDate, toDate, pageNo, pageSize);
        return response;
    }

    public async Task<BestSellingProductResponseModel> BestSellingProductsReport(DateTime fromDate, DateTime toDate, int pageNo, int pageSize)
    {
        var response = await _report.BestSellingProductsReport(fromDate, toDate, pageNo, pageSize);
        return response;
    }

    public async Task<SalesByCategoryResponseModel> SalesByCategoryReport(DateTime fromDate, DateTime toDate, int pageNo, int pageSize)
    {
        var response = await _report.SalesByCategoryReport(fromDate, toDate, pageNo, pageSize);
        return response;
    }

    private static void SaleDailyReportRequestModelCheck(SaleDailyReportRequestModel requestModel)
    {
        if (!requestModel.FromDate.HasValue )
            throw new Exception("From Date is null.");

        if (!requestModel.ToDate.HasValue)
            throw new Exception("To Date is null.");
    }
}