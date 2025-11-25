using DotNet8.Pos.App.Models.Dashboard;
using Newtonsoft.Json;

namespace DotNet8.Pos.App.Components.Pages.Dashboard;

public partial class P_Dashboard
{
    private DashboardRequestModel? _requestModel { get; set; } = new DashboardRequestModel();
    public DashboardResponseModel _responseModel { get; set; } = new DashboardResponseModel();
    public string _yearlyDate { get; set; }
    public string _yearlyAmount { get; set; }
    public string _dailyDate { get; set; }
    public string _dailyAmount { get; set; }

    private ColumnChart ColumnChartData;
    private FunnelChart FunnelChartData;

    protected override async Task OnInitializedAsync()
    {

        //await InjectService.EnableLoading();
        //await JSRuntime.InvokeVoidAsync("setLineColumnChart", ColumnChartData);
        //await JSRuntime.InvokeVoidAsync("setFunnelChart", FunnelChartData);
        //await InjectService.DisableLoading();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _requestModel.SaleInvoiceDate = DateTime.Now;
            _responseModel = await HttpClientService.ExecuteAsync<DashboardResponseModel>($"{Endpoints.Dashboard}", EnumHttpMethod.Post, _requestModel);
            Console.WriteLine(JsonConvert.SerializeObject(_responseModel).ToString());
            StateHasChanged();

            if (_responseModel != null)
            {
                _yearlyDate = _responseModel?.Data.Dashboard?.YearlyData?.FirstOrDefault()?.Year.ToString() ?? string.Empty;
                _yearlyAmount = _responseModel?.Data.Dashboard?.YearlyData?.FirstOrDefault()?.Amount.ToString() ?? "0";
                _dailyDate = _responseModel?.Data?.Dashboard?.DailyData?.FirstOrDefault()?.SaleInvoiceDate.ToString("dd-MM-yyyy") ?? string.Empty;
                _dailyAmount = _responseModel?.Data?.Dashboard?.DailyData?.FirstOrDefault()?.Amount.ToString() ?? "0";
                Console.WriteLine($"_yearlyDate{_yearlyDate} _yearlyAmount{_yearlyAmount} _dailyDate{_dailyDate} _dailyAmount{_dailyAmount}");
            }

            var productName = _responseModel.Data.Dashboard.BestSellerProduct.Select(b => b.ProductName).ToList().ToArray();
            var quantity = _responseModel.Data.Dashboard.BestSellerProduct.Select(b => b.TotalQty).ToList().ToArray();
            ColumnChartData = new ColumnChart(productName, quantity);

            var DailySaleInvoiceDate = _responseModel.Data.Dashboard.WeeklyData.Select(b => b.SaleInvoiceDate.ToString("dd/MM/yyyy")).ToList().ToArray();
            var TotalAmt = _responseModel.Data.Dashboard.WeeklyData.Select(b => b.Amount).ToList().ToArray();
            FunnelChartData = new FunnelChart(DailySaleInvoiceDate, TotalAmt);

            await InjectService.EnableLoading();
            await JSRuntime.InvokeVoidAsync("setLineColumnChart", ColumnChartData);
            //await JSRuntime.InvokeVoidAsync("setFunnelChart", FunnelChartData);
            var totalAmountList = TotalAmt.Select(x => x / 1000).ToList();
            await JSRuntime.InvokeVoidAsync("setBarChart", "#barChart", DailySaleInvoiceDate, totalAmountList);
            await InjectService.DisableLoading();
        }
    }

    public class ColumnChart
    {

        public ColumnChart(string[] productName, int[] quantity)
        {
            this.productName = productName;
            this.quantity = quantity;
        }
        public string[] productName { get; set; }

        public int[] quantity { get; set; }
    }

    public class FunnelChart
    {
        public FunnelChart(string[] salesInvoiceDate, decimal[] totalAmount)
        {
            SalesInvoiceDate = salesInvoiceDate;
            TotalAmount = totalAmount;
        }

        public string[] SalesInvoiceDate { get; set; }

        public decimal[] TotalAmount { get; set; }
    }
}