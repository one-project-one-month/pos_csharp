using DotNet8.Pos.App.Models.Staff;

namespace DotNet8.Pos.App.Components.Pages.Staff;

public partial class P_DeleteStaffDialog
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    [Parameter] public int id { get; set; }

    void Cancel() => MudDialog?.Cancel();

    private async Task DeleteAsync()
    {
        var response = await HttpClientService.ExecuteAsync<StaffResponseModel>(
            Endpoints.Staff + "/" + id,
            EnumHttpMethod.Delete,
            null);
        if (response.IsError)
        {
            InjectService.ShowMessage(response.Message, EnumResponseType.Error);
            return;
        }

        InjectService.ShowMessage(response.Message, EnumResponseType.Success);
        MudDialog.Close();
    }
}