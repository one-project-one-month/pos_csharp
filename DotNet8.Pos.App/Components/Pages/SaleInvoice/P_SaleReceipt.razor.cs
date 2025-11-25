using DotNet8.Pos.App.Models.SaleInvoice;

namespace DotNet8.Pos.App.Components.Pages.SaleInvoice;

public partial class P_SaleReceipt
{
    [Parameter]
    public string? VoucherNo { get; set; } = null!;

    public SaleInvoiceModel ResponseModel { get; set; }

    private int count = 0;

    protected override async void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            await InjectService.EnableLoading();
            await GetSaleInvoice();
            StateHasChanged();
            await InjectService.DisableLoading();
        }
    }
    private async Task GetSaleInvoice()
    {
        var response = await HttpClientService.ExecuteAsync<SaleInvoiceResponseModel>
        (
            $"{Endpoints.SaleInvoice}/{VoucherNo}",
                    EnumHttpMethod.Get
        );
        if (response != null)
        {
            ResponseModel = response.Data.SaleInvoice;
        }
    }
}

