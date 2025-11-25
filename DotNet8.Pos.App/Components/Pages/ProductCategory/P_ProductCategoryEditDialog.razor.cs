namespace DotNet8.Pos.App.Components.Pages.ProductCategory;

public partial class P_ProductCategoryEditDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    [Parameter] public ProductCategoryRequestModel reqModel { get; set; } = new();
    void Cancel() => MudDialog?.Cancel();
    private async Task SaveAsync()
    {
        if (validate())
        {
            var response = await HttpClientService.ExecuteAsync<ProductResponseModel>(
                Endpoints.ProductCategory + $"/{reqModel.ProductCategoryId}",
                EnumHttpMethod.Patch,
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
    private bool validate()
    {
        if (string.IsNullOrEmpty(reqModel.ProductCategoryName))
        {
            ShowWarningMessage("Product Name is required.");
            return false;
        }
        if (string.IsNullOrEmpty(reqModel.ProductCategoryCode))
        {
            ShowWarningMessage("Product Category Code is required.");
            return false;
        }            
        return true;
    }
    private void ShowWarningMessage(string message)
    {
        InjectService.ShowMessage(message, EnumResponseType.Warning);
    }
}