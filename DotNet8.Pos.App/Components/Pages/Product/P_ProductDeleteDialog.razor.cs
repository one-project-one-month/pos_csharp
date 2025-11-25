namespace DotNet8.Pos.App.Components.Pages.Product;

public partial class P_ProductDeleteDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public string contentText { get; set; }
    [Parameter] public int productId { get; set; }

    private ProductRequestModel reqModel = new();
    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task DeleteProduct(int id)
    {
        var reqModel = await HttpClientService.ExecuteAsync<ProductListResponseModel>(Endpoints.Product, EnumHttpMethod.Get);
        if (reqModel is not null)
        {
            await HttpClientService.ExecuteAsync<TownshipResponseModel>(
                Endpoints.Product+$"/{id}",
                EnumHttpMethod.Delete
            );
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            InjectService.ShowMessage("No data found.", EnumResponseType.Warning);
        }
    }
}