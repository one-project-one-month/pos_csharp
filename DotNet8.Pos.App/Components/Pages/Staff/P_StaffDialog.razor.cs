

using DotNet8.Pos.App.Models.Staff;
using DotNet8.PosBackendApi.Shared;

namespace DotNet8.Pos.App.Components.Pages.Staff;

public partial class P_StaffDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    public StaffRequestModel _reqModel { get; set; } = new();

    [Parameter] public int _staffId { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await GetStaff();
    }
    private async Task GetStaff()
    {
        Console.WriteLine(_staffId);
        if (_staffId > 0)
        {
            var staff = await HttpClientService.ExecuteAsync<StaffResponseModel>(
                $"{Endpoints.Staff}/ {_staffId}",
                EnumHttpMethod.Get
            );
            _reqModel = new StaffRequestModel
            {
                StaffName = staff.Data.Staff.StaffName,
                StaffCode = staff.Data.Staff.StaffCode,
                Address = staff.Data.Staff.Address,
                DateOfBirth = staff.Data.Staff.DateOfBirth,
                Gender = staff.Data.Staff.Gender,
                MobileNo = staff.Data.Staff.MobileNo,
                Password = staff.Data.Staff.Password,
                Position = staff.Data.Staff.Position,
            };
        }
    }
    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task SaveAsync()
    {
        _reqModel.StaffCode = "";
        DateTime StaffDOB = _reqModel.DateOfBirth.ToDateTime();
        DateTime DateTimeNow = DateTime.Now;
        TimeSpan ageSpan = DateTimeNow - StaffDOB;
        int StaffAge = (int)(ageSpan.Days / 365.25);
        if (StaffAge < 18 || StaffAge > 45)
        {
            //await JSRuntime.InvokeVoidAsync("alert", "Staff Age must be greater than 18 years.");
            InjectService.ShowMessage("Staff Age must have between 18 and 45.", EnumResponseType.Information);
            return;
        }

        if(_reqModel.MobileNo.Length != 11)
        {
            InjectService.ShowMessage("MobileNo must have 11 Digit.", EnumResponseType.Information);
            return;
        }
        else
        {
            var respondStaff = await HttpClientService.ExecuteAsync<StaffResponseModel>(
                $"{Endpoints.Staff}/GetStaffByMobileNo/{_reqModel.MobileNo}",
                EnumHttpMethod.Get);

            if (respondStaff.Data is not null)
            {
                InjectService.ShowMessage("Please use another Mobile No.", EnumResponseType.Error);
                return;
            }
        }

        var response = await HttpClientService.ExecuteAsync<StaffResponseModel>(
            Endpoints.Staff,
            EnumHttpMethod.Post,
            _reqModel
        );

        if (response.IsError)
        {
            Console.WriteLine(response.Message);
            Console.WriteLine(EnumResponseType.Error);
            InjectService.ShowMessage(response.Message, EnumResponseType.Error);
            return;
        }

        InjectService.ShowMessage(response.Message, EnumResponseType.Success);
        MudDialog.Close();
    }
}