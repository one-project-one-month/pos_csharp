using DotNet8.Pos.App.Models.SaleInvoice;
using Newtonsoft.Json;

namespace DotNet8.Pos.App.Components.Pages.SaleInvoice;

public partial class P_SaleInvoiceList
{
    private SaleInvoiceListResponseModel? ResponseModel;
    private int pageNo = 1;
    private int pageSize = 10;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await InjectService.EnableLoading();
            await List();
            StateHasChanged();
            await InjectService.DisableLoading();
        }
    }

    private async Task List()
    {
        ResponseModel = await HttpClientService.ExecuteAsync<SaleInvoiceListResponseModel>
        (
            Endpoints.SaleInvoice.WithPagination(pageNo, pageSize),
            EnumHttpMethod.Get
        );
        Console.WriteLine(JsonConvert.SerializeObject(ResponseModel));
    }

    private void Create()
    {
        Nav.NavigateTo("sale-invoice");
    }

    private async Task PageChanged(int i)
    {
        pageNo = i;
        await List();
    }

    private async Task Popup(string voucherNo)
    {
        DialogResult result;

        var parameters = new DialogParameters<P_SaleInvoiceDetailDialog>();
        parameters.Add("voucherNo", voucherNo);

        result = await InjectService.ShowModalBoxAsync<P_SaleInvoiceDetailDialog>("Sale Invoice Detail", parameters);

        if (!result.Canceled)
        {
            await List();
            StateHasChanged();
        }
    }
}