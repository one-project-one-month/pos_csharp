namespace DotNet8.Pos.App.Components.Pages.State;

public partial class P_StateDeleteDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public string contentText { get; set; }
    [Parameter] public string buttonText { get; set; }
    [Parameter] public Color color { get; set; }
    [Parameter] public int stateId { get; set; }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async void DeleteState()
    {
           
        var response = await HttpClientService.ExecuteAsync<StateResponeModel>(
            Endpoints.State + "/" + stateId,
            EnumHttpMethod.Delete,
            null);
        if (response.IsError)
        {
            InjectService.ShowMessage(response.Message,EnumResponseType.Error);
        }
        InjectService.ShowMessage(response.Message, EnumResponseType.Success);
        MudDialog.Close();
    }

}