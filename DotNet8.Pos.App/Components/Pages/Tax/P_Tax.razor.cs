using DotNet8.Pos.App.Models.Tax;

namespace DotNet8.Pos.App.Components.Pages.Tax;

public partial class P_Tax
{
    public int _pageNo = 1;
    public int _pageSize = 10;

    public TaxListResponseModel? responseModel;

    protected override async void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            await InjectService.EnableLoading();
            await List();
            StateHasChanged();
            await InjectService.DisableLoading();
        }
    }

    public async Task List()
    {
        responseModel = await HttpClientService.ExecuteAsync<TaxListResponseModel>(
            $"{Endpoints.Tax}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get);
    }

    public async Task CreatePopUp()
    {
        DialogResult dialogResult = await InjectService.ShowModalBoxAsync<P_CreateTaxDialog>("New Tax");

        if (!dialogResult.Canceled)
            await List();
    }

    public async Task EditPopUp(int id, int? fromAmount, int? toAmount, decimal? percentage, decimal? fixedAmount, string taxType)
    {
        TaxModel model = new()
        {
            TaxId = id,
            FromAmount = fromAmount,
            ToAmount = toAmount,
            TaxType = taxType,
            Percentage = percentage,
            FixedAmount = fixedAmount
        };
        DialogParameters parameters = new DialogParameters<P_EditTaxDialog>()
        {
            {x => x.requestModel, model },
            {x => x.showPercentageField, model.Percentage == 0 ? false : true },
            {x => x.showFixedAmountField, model.FixedAmount == 0 ? false : true}
        };

        DialogResult dialogResult = await InjectService.ShowModalBoxAsync<P_EditTaxDialog>("Edit Tax", parameters);

        if (!dialogResult.Canceled)
            await List();
    }

    public async Task DeletePopUp(int id)
    {
        DialogParameters parameters = new DialogParameters<P_DeleteTaxDialog>()
        {
            { x => x.id, id }
        };
        DialogResult result = await InjectService.ShowModalBoxAsync<P_DeleteTaxDialog>("Delete Tax", parameters);

        if (!result.Canceled)
            await List();
    }

    public async Task PageChanged(int i)
    {
        _pageNo = i;
        await List();
    }
}