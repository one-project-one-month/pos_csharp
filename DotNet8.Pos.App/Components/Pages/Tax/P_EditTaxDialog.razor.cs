using DotNet8.Pos.App.Models.Tax;
using static DotNet8.Pos.App.Components.Pages.Tax.P_CreateTaxDialog;

namespace DotNet8.Pos.App.Components.Pages.Tax;

public partial class P_EditTaxDialog
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    [Parameter] public TaxModel requestModel { get; set; } = new();

    [Parameter] public bool showPercentageField { get; set; } = false;

    [Parameter] public bool showFixedAmountField { get; set; } = false;

    void Cancel() => MudDialog?.Cancel();

    public async Task SaveAsync()
    {
        #region Validation

        if (requestModel.FromAmount <= 0 || requestModel.FromAmount is null)
        {
            InjectService.ShowMessage("From Amount is invalid.", EnumResponseType.Warning);
            return;
        }

        if (requestModel.ToAmount <= 0 || requestModel.ToAmount is null)
        {
            InjectService.ShowMessage("To Amount is invalid.", EnumResponseType.Warning);
            return;
        }

        if (requestModel.FromAmount >= requestModel.ToAmount)
        {
            InjectService.ShowMessage("From Amount must be greater than To Amount", EnumResponseType.Warning);
            return;
        }

        if (string.IsNullOrEmpty(requestModel.TaxType))
        {
            InjectService.ShowMessage("Tax Type cannot be empty.", EnumResponseType.Warning);
            return;
        }

        if (requestModel.Percentage is null && requestModel.FixedAmount is null)
        {
            InjectService.ShowMessage("Please fill all fields...", EnumResponseType.Warning);
            return;
        }

        if (requestModel.Percentage != 0 && requestModel.Percentage is not null)
        {
            if (requestModel.Percentage <= 0 || requestModel.Percentage >= 100)
            {
                InjectService.ShowMessage("Invalid Percentage.", EnumResponseType.Warning);
                return;
            }
        }

        if (requestModel.FixedAmount is not null && requestModel.FixedAmount != 0)
        {
            if (requestModel.FixedAmount <= 0)
            {
                InjectService.ShowMessage("Invalid Fixed Amount.", EnumResponseType.Warning);
                return;
            }
        }

        #endregion

        requestModel.Percentage = Convert.ToDecimal(requestModel.Percentage);
        requestModel.FixedAmount = Convert.ToDecimal(requestModel.FixedAmount);

        var response = await HttpClientService.ExecuteAsync<TaxResponseModel>(
            $"{Endpoints.Tax}/{requestModel.TaxId}",
            EnumHttpMethod.Patch,
            requestModel);

        if (response.IsError)
        {
            InjectService.ShowMessage(response.Message, EnumResponseType.Error);
            return;
        }

        InjectService.ShowMessage(response.Message, EnumResponseType.Success);
        MudDialog?.Close();
    }

    private void TaxTypeChanged(string taxType)
    {
        if (taxType == nameof(EnumTaxType.Percentage))
        {
            showPercentageField = true;
            showFixedAmountField = false;
            requestModel.FixedAmount = null;
        }
        else
        {
            showFixedAmountField = true;
            showPercentageField = false;
            requestModel.Percentage = null;
        }
    }
}
