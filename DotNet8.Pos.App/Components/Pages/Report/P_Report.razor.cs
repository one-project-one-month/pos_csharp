using DotNet8.Pos.App.Models.Report;

namespace DotNet8.Pos.App.Components.Pages.Report;

public partial class P_Report
{
    private int _pageNo = 1;
    private int _pageSize = 10;

    private ReportListResponseModel? responseModel;
    private EnumReportDate dateFormat { get; set; }
    private string? fromDate {  get; set; }
    private string? toDate { get; set; }
    private async Task FromDateChanged(DateTime? newDate)
    {
        fromDate = newDate?.ToString("yyyy-MMM-dd") ?? DateTime.Today.ToString("yyyy-MMM-dd");
        toDate = !string.IsNullOrEmpty(toDate) ? toDate : DateTime.Today.ToString("yyyy-MMM-dd");
        await GetReportData();
    }

    private async Task ToDateChanged(DateTime? newDate)
    {
        toDate = newDate?.ToString("yyyy-MMM-dd") ?? DateTime.Today.ToString("yyyy-MMM-dd");
        fromDate = !string.IsNullOrEmpty(fromDate) ? fromDate : DateTime.Today.ToString("yyyy-MMM-dd");
        await GetReportData();
    }

    private async Task DateRangeChanged(DateRange? newDateRange)
    {
        fromDate = newDateRange?.Start?.ToString("yyyy-MMM-dd");
        toDate = newDateRange?.End?.ToString("yyyy-MMM-dd");
        await GetReportData();
    }

    private async Task PageChanged(int i)
    {
        _pageNo = i;
        await GetReportData();
    }

    private void OnValueChanged()
    {
        switch (dateFormat)
        {
            case EnumReportDate.Daily:
                responseModel = null;
                break;
            case EnumReportDate.Monthly:
                responseModel = null;
                break;
            case EnumReportDate.Yearly:
                responseModel = null;
                break;
            case EnumReportDate.None:
                responseModel = null;
                break;
        }
    }

    private async Task GetReportData()
    {
        await InjectService.EnableLoading();
        switch (dateFormat)
        {
            case EnumReportDate.Daily:
                await ReportDaily();
                break;
            case EnumReportDate.Monthly:
                await ReportMonthly();
                break;
            case EnumReportDate.Yearly:
                await ReportYearly();
                break;
            case EnumReportDate.None:
                responseModel = null;
                break;
        }
        StateHasChanged();
        await InjectService.DisableLoading();
    }

    private async Task ReportDaily()
    {
        responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
            $"{Endpoints.Report}/daily-report/{fromDate}/{toDate}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
        );
    }

    private async Task ReportMonthly()
    {
        responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
            $"{Endpoints.Report}/monthly-report/{fromDate}/{toDate}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
        );
    }

    private async Task ReportYearly()
    {
        responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
            $"{Endpoints.Report}/yearly-report/{fromDate}/{toDate}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
        );
    }

    /*private async Task ReportDaily()
    {
        _dateDay = DateValue?.Day;
        _dateMonth = DateValue?.Month;
        _dateYear = DateValue?.Year;
        responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
        $"{Endpoints.Report}/daily-report/{_dateDay}/{_dateMonth}/{_dateYear}/{_pageNo}/{_pageSize}",
        EnumHttpMethod.Get
        );
    }
    private async Task ReportMonthly()
    {
        _dateMonth = DateValue?.Month;
        _dateYear = DateValue?.Year;
        responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
        $"{Endpoints.Report}/monthly-report/{_dateMonth}/{_dateYear}/{_pageNo}/{_pageSize}",
        EnumHttpMethod.Get
        );
    }
    private async Task ReportYearly()
    {
        _dateMonth = DateValue?.Month;
        _dateYear = DateValue?.Year;
        responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
        $"{Endpoints.Report}/yearly-report/{_dateYear}/{_pageNo}/{_pageSize}",
        EnumHttpMethod.Get
        );
    }*/
}