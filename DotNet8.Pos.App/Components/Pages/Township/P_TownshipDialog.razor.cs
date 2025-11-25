namespace DotNet8.Pos.App.Components.Pages.Township;

public partial class P_TownshipDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public TownshipModel model { get; set; }

    private TownshipModel reqModel = new TownshipModel();

    private StateListResponseModel lstStateCode = new StateListResponseModel();

    protected override async void OnInitialized()
    {
        lstStateCode = await HttpClientService.ExecuteAsync<StateListResponseModel>(
            Endpoints.State,
            EnumHttpMethod.Get);
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task SaveAsync()
    {
        var response = await HttpClientService.ExecuteAsync<TownshipResponseModel>(
            Endpoints.Township,
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
        var response = await HttpClientService.ExecuteAsync<TownshipResponseModel>(
            $"{Endpoints.Township}/{model.TownshipId}",
            EnumHttpMethod.Patch,
            model
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