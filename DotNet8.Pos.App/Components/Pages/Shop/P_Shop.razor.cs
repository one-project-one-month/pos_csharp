using DotNet8.Pos.App.Models.Shop;

namespace DotNet8.Pos.App.Components.Pages.Shop;

public partial class P_Shop
{
    private int _pageNo = 1;
    private int _pageSize = 10;

    private ShopListResponseModel? responseModel;
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

    private async Task List()
    {
        responseModel = await HttpClientService.ExecuteAsync<ShopListResponseModel>(
            $"{Endpoints.Shop}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
        );
    }

    private async Task Popup()
    {
        var result = await InjectService.ShowModalBoxAsync<P_ShopDialog>("New Shop");
        if (!result.Canceled)
        {
            await List();
        }
    }

    private async Task EditPopUp(ShopModel shop)
    {
        ShopRequestModel? requestModel = new()
        {
            ShopId = shop.ShopId,
            ShopCode = shop.ShopCode,
            ShopName = shop.ShopName,
            MobileNo = shop.MobileNo,
            Address = shop.Address
        };
        DialogParameters parameters = new DialogParameters<P_ShopEditDialog>()
        {
            {x => x.requestModel, requestModel }
        };
        DialogResult dialogResult = await InjectService.ShowModalBoxAsync<P_ShopEditDialog>("Edit Shop", parameters);

        if (!dialogResult.Canceled)
            await List();
    }

    private async Task Delete(int id)
    {
        var parameters = new DialogParameters<P_ShopDeleteDialog>();
        parameters.Add(x => x.contentText, "Are you sure you want to delete?");
        parameters.Add(x => x.shopId, id);

        var options = new MudBlazor.DialogOptions()
        {
            CloseButton = true,
            MaxWidth = MaxWidth.ExtraSmall
        };

        var result = await InjectService.ShowModalBoxAsync<P_ShopDeleteDialog>("Delete", parameters);
        if (!result.Canceled)
        {
            await List();
        }
    }

    public async Task PageChanged(int i)
    {
        _pageNo = i;
        await List();
    }
}