using DotNet8.Pos.App.Models.Tax;

namespace DotNet8.Pos.App.Components.Pages.Tax;

public partial class P_CreateTaxDialog
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    public bool showPercentageField = false;

    public bool showFixedAmountField = false;

    TaxModel requestModel = new();

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

        requestModel.FromAmount = Convert.ToInt32(requestModel.FromAmount);
        requestModel.ToAmount = Convert.ToInt32(requestModel.ToAmount);
        requestModel.Percentage = Convert.ToDecimal(requestModel.Percentage);
        requestModel.FixedAmount = Convert.ToDecimal(requestModel.FixedAmount);

        var response = await HttpClientService.ExecuteAsync<TaxResponseModel>(
            Endpoints.Tax,
            EnumHttpMethod.Post,
            requestModel);

        if (response.IsError)
        {
            InjectService.ShowMessage(response.Message, EnumResponseType.Error);
            return;
        }

        InjectService.ShowMessage(response.Message, EnumResponseType.Success);
        MudDialog?.Close();
    }

    private void TaxTypeChanged(string tax)
    {
        if (tax == nameof(EnumTaxType.Percentage))
        {
            showPercentageField = true;
            showFixedAmountField = false;
        }
        else
        {
            showFixedAmountField = true;
            showPercentageField = false;
        }
    }

    public enum EnumTaxType
    {
        None,
        Percentage,
        Fixed
    }
}