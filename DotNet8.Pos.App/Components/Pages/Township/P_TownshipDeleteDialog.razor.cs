namespace DotNet8.Pos.App.Components.Pages.Township;

public partial class P_TownshipDeleteDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public string contentText { get; set; }
    [Parameter] public string buttonText { get; set; }
    [Parameter] public Color color { get; set; }
    [Parameter] public int townshipId { get; set; }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async void DeleteTownship()
    {
        var response = await HttpClientService.ExecuteAsync<TownshipResponseModel>(
            Endpoints.Township + "/" + townshipId,
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