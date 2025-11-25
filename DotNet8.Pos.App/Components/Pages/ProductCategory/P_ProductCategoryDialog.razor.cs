namespace DotNet8.Pos.App.Components.Pages.ProductCategory;

public partial class P_ProductCategoryDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    private ProductCategoryRequestModel reqModel = new();

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task SaveAsync()
    {
        var response = await HttpClientService.ExecuteAsync<ProductCategoryResponseModel>(
            Endpoints.ProductCategory,
            EnumHttpMethod.Post,
            reqModel
        );
        if (response.IsError)
        {
            InjectService.ShowMessage(response.Message, EnumResponseType.Error);
            return;
        }

        InjectService.ShowMessage(response.Message, EnumResponseType.Success);
        MudDialog.Close();
    }
}
