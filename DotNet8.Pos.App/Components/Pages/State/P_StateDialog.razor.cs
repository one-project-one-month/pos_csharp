namespace DotNet8.Pos.App.Components.Pages.State;

public partial class P_StateDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public StateModel model { get; set; }
    private StateModel reqModel = new();

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task SaveAsync()
    {
        var response = await HttpClientService.ExecuteAsync<StateResponeModel>(
            Endpoints.State,
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

    private async Task EditAsync()
    {
        var response = await HttpClientService.ExecuteAsync<StateResponeModel>(
            $"{Endpoints.State}/{model.StateId}", 
            EnumHttpMethod.Patch,
            model
        );
        if (response.IsError)
        {
            InjectService.ShowMessage(response.Message,EnumResponseType.Error);
            return;
        }
        InjectService.ShowMessage(response.Message, EnumResponseType.Success);
        MudDialog.Close();
    }

        
}