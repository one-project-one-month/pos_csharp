using DotNet8.Pos.App.Models.Report;
using DotNet8.Pos.App.Models.Custom;
using Microsoft.AspNetCore.Components;

namespace DotNet8.Pos.App.Components.Pages.Report;

public partial class P_Report
{
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private int _pageNo = 1;
    private int _pageSize = 10;

    private ReportListResponseModel? responseModel;
    private BestSellingProductListResponseModel? bestSellingProductResponseModel;
    private SalesByCategoryListResponseModel? salesByCategoryResponseModel;
    private EnumReportDate dateFormat { get; set; }
    private string? fromDate {  get; set; }
    private string? toDate { get; set; }

    // Monthly report specific filters
    private int? currentMonthFilter { get; set; }
    private int currentYearFilter { get; set; }

    // UI bound properties
    public DateTime? startDateValue { get; set; }
    public DateTime? endDateValue { get; set; }
    public DateRange? dateRangeValue { get; set; }
    public int selectedYear { get; set; }
    public int? selectedMonth { get; set; }
    public int selectedMonthlyYear { get; set; }

    // Available years for yearly report (last 10 years to next year)
    public List<int> AvailableYears => Enumerable.Range(DateTime.Now.Year - 10, 12).ToList();

    // Available months for monthly report
    public Dictionary<int, string> AvailableMonths => new Dictionary<int, string>
    {
        { 1, "January" },
        { 2, "February" },
        { 3, "March" },
        { 4, "April" },
        { 5, "May" },
        { 6, "June" },
        { 7, "July" },
        { 8, "August" },
        { 9, "September" },
        { 10, "October" },
        { 11, "November" },
        { 12, "December" }
    };
    private async Task ApplyFilter()
    {
        await InjectService.EnableLoading();

        // Set date values based on report type
        switch (dateFormat)
        {
            case EnumReportDate.Daily:
                fromDate = startDateValue?.ToString("yyyy-MM-dd");
                toDate = endDateValue?.ToString("yyyy-MM-dd");
                break;
            case EnumReportDate.Monthly:
                // Store month and year separately for the API call
                // If no month is selected, we'll use yearly endpoint instead
                currentMonthFilter = selectedMonth;
                currentYearFilter = selectedMonthlyYear;
                break;
            case EnumReportDate.Yearly:
                // Yearly reports don't use fromDate/toDate - they use selectedYear directly in the API call
                break;
            case EnumReportDate.BestSellingProducts:
            case EnumReportDate.SalesByCategory:
                fromDate = dateRangeValue?.Start?.ToString("yyyy-MM-dd");
                toDate = dateRangeValue?.End?.ToString("yyyy-MM-dd");
                break;
        }

        await GetReportData();
        await InjectService.DisableLoading();
    }

    private async Task PageChanged(int i)
    {
        _pageNo = i;
        await GetReportData();
    }

    private void OnValueChanged()
    {
        responseModel = null;
        bestSellingProductResponseModel = null;
        salesByCategoryResponseModel = null;

        // Clear UI values when report type changes
        startDateValue = null;
        endDateValue = null;
        dateRangeValue = null;
        selectedYear = DateTime.Now.Year; // Default to current year
        selectedMonth = null; // Default to None
        selectedMonthlyYear = DateTime.Now.Year; // Default to current year for monthly reports
        currentMonthFilter = null;
        currentYearFilter = DateTime.Now.Year;
        fromDate = null;
        toDate = null;
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
                // If no month is selected, use yearly filter instead
                if (!currentMonthFilter.HasValue)
                {
                    await ReportYearly();
                }
                else
                {
                    await ReportMonthly();
                }
                break;
            case EnumReportDate.Yearly:
                await ReportYearly();
                break;
            case EnumReportDate.BestSellingProducts:
                await ReportBestSellingProducts();
                break;
            case EnumReportDate.SalesByCategory:
                await ReportSalesByCategory();
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
            $"{Endpoints.Report}/monthly-report/{currentMonthFilter}/{currentYearFilter}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
        );
    }

    private async Task ReportYearly()
    {
        // Use currentYearFilter if we're in monthly mode with no month selected, otherwise use selectedYear
        var yearToUse = dateFormat == EnumReportDate.Monthly && !currentMonthFilter.HasValue
            ? currentYearFilter
            : selectedYear;

        responseModel = await HttpClientService.ExecuteAsync<ReportListResponseModel>(
            $"{Endpoints.Report}/yearly-report/{yearToUse}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
        );
    }

    private async Task ReportBestSellingProducts()
    {
        bestSellingProductResponseModel = await HttpClientService.ExecuteAsync<BestSellingProductListResponseModel>(
            $"{Endpoints.Report}/best-selling-products/{fromDate}/{toDate}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
        );
    }

    private async Task ReportSalesByCategory()
    {
        salesByCategoryResponseModel = await HttpClientService.ExecuteAsync<SalesByCategoryListResponseModel>(
            $"{Endpoints.Report}/sales-by-category/{fromDate}/{toDate}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
        );
    }

    protected override async Task OnInitializedAsync()
    {
        await UpdateReportTypeFromQuery();
    }

    protected override async Task OnParametersSetAsync()
    {
        await UpdateReportTypeFromQuery();
    }

    private async Task UpdateReportTypeFromQuery()
    {
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

        if (query.TryGetValue("type", out var type))
        {
            var newDateFormat = type.ToString().ToLower() switch
            {
                "daily" => EnumReportDate.Daily,
                "monthly" => EnumReportDate.Monthly,
                "yearly" => EnumReportDate.Yearly,
                "best-selling" => EnumReportDate.BestSellingProducts,
                "sales-by-category" => EnumReportDate.SalesByCategory,
                _ => EnumReportDate.None
            };

            if (dateFormat != newDateFormat)
            {
                dateFormat = newDateFormat;
                // Clear existing data when switching report types
                OnValueChanged();

                // Set default values for yearly and monthly reports
                if (dateFormat == EnumReportDate.Yearly)
                {
                    selectedYear = DateTime.Now.Year;
                }
                else if (dateFormat == EnumReportDate.Monthly)
                {
                    selectedMonthlyYear = DateTime.Now.Year;
                    selectedMonth = null; // Default to None
                    currentMonthFilter = null;
                    currentYearFilter = DateTime.Now.Year;
                }

                StateHasChanged();
            }
        }
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