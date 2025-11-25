using DotNet8.Pos.App.Models.Shop;

namespace DotNet8.Pos.App.Components.Pages.Shop;

public partial class P_Shop
{
    private int pageNo = 1;
    private int pageSize = 10;
    private string searchText = string.Empty;

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
        var url = Endpoints.Shop.WithPagination(pageNo, pageSize);
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            url += $"&search={Uri.EscapeDataString(searchText)}";
        }

        responseModel = await HttpClientService.ExecuteAsync<ShopListResponseModel>(
            url,
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

    private async Task PageChanged(int i)
    {
        pageNo = i;
        await List();
    }

    private async Task Search()
    {
        pageNo = 1; // Reset to first page when searching
        await List();
    }

    private async Task ClearSearch()
    {
        searchText = string.Empty;
        pageNo = 1; // Reset to first page when clearing search
        await List();
    }

    private async Task HandleSearchKeyUp(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await Search();
        }
    }
}