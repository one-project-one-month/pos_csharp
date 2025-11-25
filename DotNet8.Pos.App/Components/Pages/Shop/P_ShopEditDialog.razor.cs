using DotNet8.Pos.App.Models.Shop;

namespace DotNet8.Pos.App.Components.Pages.Shop;

public partial class P_ShopEditDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    [Parameter] public ShopRequestModel requestModel { get; set; } = new();

    void Cancel() => MudDialog?.Cancel();

    private async Task SaveAsync()
    {
        if (validate())
        {
            var response = await HttpClientService.ExecuteAsync<ShopResponseModel>(
                Endpoints.Shop + $"/{requestModel.ShopId}",
                EnumHttpMethod.Patch,
                requestModel
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
        if (string.IsNullOrEmpty(requestModel.ShopCode))
        {
            ShowWarningMessage("Shop Code is required.");
            return false;
        }
        if (string.IsNullOrEmpty(requestModel.ShopName))
        {
            ShowWarningMessage("Shop Name is required.");
            return false;
        }
        if (string.IsNullOrEmpty(requestModel.MobileNo))
        {
            ShowWarningMessage("Mobile Number is required.");
            return false;
        }
        if (string.IsNullOrEmpty(requestModel.Address))
        {
            ShowWarningMessage("Address is required.");
            return false;
        }
        return true;
    }

    private void ShowWarningMessage(string message)
    {
        InjectService.ShowMessage(message, EnumResponseType.Warning);
    }
}