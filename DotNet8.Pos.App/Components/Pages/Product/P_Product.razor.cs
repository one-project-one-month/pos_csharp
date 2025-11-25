namespace DotNet8.Pos.App.Components.Pages.Product;

public partial class P_Product
{
    public int _pageNo = 1;
    public int _pageSize = 10;
    private ProductListResponseModel? ResponseModel;

    protected override async Task OnAfterRenderAsync(bool firstRender)
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
        ResponseModel = await HttpClientService.ExecuteAsync<ProductListResponseModel>(
            $"{Endpoints.Product}/{_pageNo}/{_pageSize}",
            EnumHttpMethod.Get
        );
    }

    private async Task Popup(string title)
    {
        //MudBlazor.DialogOptions maxWidth = new MudBlazor.DialogOptions() { MaxWidth = MaxWidth.Small, FullWidth = true };
        //var dialog = await DialogService.ShowAsync<P_ProductDialog>("New Product", maxWidth);
        //var result = await dialog.Result;

        var result = await InjectService.ShowModalBoxAsync<P_ProductDialog>(title);
        if (!result.Canceled)
        {
            await List();
        }
    }

    private async Task EditPopUp(int id, string? ProductCode, string? ProductName, string? ProductCategoryCode, decimal Price)
    {
        ProductRequestModel? model = new()
        {
            ProductId = id,
            ProductCode = ProductCode,
            ProductName = ProductName,
            ProductCategoryCode = ProductCategoryCode,
            Price = Price
        };
        DialogParameters parameters = new DialogParameters<P_ProductEditDialog>()
        {
            {x => x.requestModel, model }
        };
        DialogResult dialogResult = await InjectService.ShowModalBoxAsync<P_ProductEditDialog>("Edit Product", parameters);

        if (!dialogResult.Canceled)
            await List();
    }

    private async Task Delete(int id)
    {
        var parameters = new DialogParameters<P_ProductDeleteDialog>();
        parameters.Add(x => x.contentText, "Are you sure you want to delete?");
        parameters.Add(x => x.productId, id);

        var options = new MudBlazor.DialogOptions()
        {
            CloseButton = true,
            MaxWidth = MaxWidth.ExtraSmall
        };

        var result = await InjectService.ShowModalBoxAsync<P_ProductDeleteDialog>("Delete", parameters);
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