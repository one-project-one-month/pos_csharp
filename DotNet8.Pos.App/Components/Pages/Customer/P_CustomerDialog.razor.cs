namespace DotNet8.Pos.App.Components.Pages.Customer;

public partial class P_CustomerDialog
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    private CustomerRequestModel requestModel = new();

    [Parameter] public StateListResponseModel stateListResponseModel { get; set; } = new();

    [Parameter] public TownshipListResponseModel townshipListResponseModel { get; set; } = new();

    [Parameter] public CustomerParamsModel model { get; set; }

    private void Cancel() => MudDialog?.Cancel();

    private bool isDisabled = true;

    protected async override Task OnInitializedAsync()
    {
        if (model is null)
        {
            stateListResponseModel = await HttpClientService.ExecuteAsync<StateListResponseModel>(
                Endpoints.State,
                EnumHttpMethod.Get);
        }
    }

    private async Task SaveAsync()
    {
        #region Validation
        if (string.IsNullOrEmpty(requestModel.CustomerName))
        {
            ShowWarningMessage("Customer Name is required.");
            return;
        }

        if (string.IsNullOrEmpty(requestModel.MobileNo))
        {
            ShowWarningMessage("Mobile Number is required.");
            return;
        }

        if (requestModel.MobileNo.Trim().Length > 11 || requestModel.MobileNo.Trim().Length < 11)
        {
            ShowWarningMessage("Invalid Mobile Number.");
            return;
        }

        if (string.IsNullOrEmpty(requestModel.Gender))
        {
            ShowWarningMessage("Gender is required.");
            return;
        }

        if (requestModel.Gender == "0")
        {
            ShowWarningMessage("Invalid Gender.");
            return;
        }

        if (requestModel.DateOfBirth is null)
        {
            ShowWarningMessage("Date Of Birth is required.");
            return;
        }


        DateTime now = DateTime.Today;
        TimeSpan ageDifference = now - Convert.ToDateTime(requestModel.DateOfBirth);
        int age = Convert.ToInt32(ageDifference.TotalDays / 365);

        if (age <= 18 || age >= 40)
        {
            ShowWarningMessage("Age must be between 18 and 45.");
            return;
        }

        if (string.IsNullOrEmpty(requestModel.StateCode))
        {
            ShowWarningMessage("State Code is required.");
            return;
        }

        if (string.IsNullOrEmpty(requestModel.TownshipCode))
        {
            ShowWarningMessage("Township Code is required.");
            return;
        }
        #endregion

        var response = await HttpClientService.ExecuteAsync<CustomerResponseModel>(
            Endpoints.Customer,
            EnumHttpMethod.Post,
            requestModel
        );

        if (response.IsError)
        {
            InjectService.ShowMessage(response.Message, EnumResponseType.Error);
            return;
        }

        InjectService.ShowMessage(response.Message, EnumResponseType.Success);
        MudDialog?.Close();
    }

    private async Task UpdateAsync()
    {
        #region Validation

        if (string.IsNullOrEmpty(model.CustomerName))
        {
            ShowWarningMessage("Customer Name cannot be empty.");
            return;
        }

        if (string.IsNullOrEmpty(model.MobileNo))
        {
            ShowWarningMessage("Mobile Number cannot be empty.");
            return;
        }

        if (model.MobileNo.Trim().Length > 11 || model.MobileNo.Trim().Length < 11)
        {
            ShowWarningMessage("Invalid Mobile Number.");
            return;
        }

        if (string.IsNullOrEmpty(model.Gender))
        {
            ShowWarningMessage("Gender is required.");
            return;
        }

        if (model.Gender == "0")
        {
            ShowWarningMessage("Invalid Gender.");
            return;
        }

        if (model.DateOfBirth is null)
        {
            ShowWarningMessage("Date Of Birth is required.");
            return;
        }


        DateTime now = DateTime.Today;
        TimeSpan ageDifference = now - Convert.ToDateTime(model.DateOfBirth);
        int age = Convert.ToInt32(ageDifference.TotalDays / 365);

        if (age <= 18 || age >= 40)
        {
            ShowWarningMessage("Age must be between 18 and 45.");
            return;
        }

        bool isTownshipValid = false;

        foreach (var item in townshipListResponseModel.Data.Township)
        {
            if (model.TownshipCode.Equals(item.TownshipCode))
            {
                isTownshipValid = true;
                break;
            }
        }

        if (!isTownshipValid)
        {
            ShowWarningMessage("Invalid Township!");
            model.TownshipCode = string.Empty;
            return;
        }

        #endregion

        CustomerRequestModel requestModel = new()
        {
            CustomerName = model.CustomerName,
            MobileNo = model.MobileNo,
            DateOfBirth = model.DateOfBirth,
            Gender = model.Gender,
            StateCode = model.StateCode,
            TownshipCode = model.TownshipCode
        };

        var response = await HttpClientService.ExecuteAsync<CustomerResponseModel>(
            $"{Endpoints.Customer}/{model.CustomerId}",
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

    private void ShowWarningMessage(string message)
    {
        InjectService.ShowMessage(message, EnumResponseType.Warning);
    }

    private async Task FetchTownship(string stateCode)
    {
        townshipListResponseModel = await HttpClientService.ExecuteAsync<TownshipListResponseModel>(
             $"{Endpoints.Township}/GetTownshipByStateCode/{stateCode}",
             EnumHttpMethod.Get);

        isDisabled = false;
    }
}