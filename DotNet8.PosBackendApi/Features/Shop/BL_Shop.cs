namespace DotNet8.PosBackendApi.Features.Shop;

public class BL_Shop
{
    private readonly DL_Shop _dL_Shop;

    public BL_Shop(DL_Shop dL_Shop) => _dL_Shop = dL_Shop;

    public async Task<ShopListResponseModel> GetShops()
    {
        var response = await _dL_Shop.GetShops();
        return response;
    }
    public async Task<ShopListResponseModel> GetShops(int pageNo, int pageSize, string? search = null)
    {
        var response = await _dL_Shop.GetShops(pageNo, pageSize, search);
        return response;
    }
    public async Task<ShopResponseModel> GetShop(int id)
    {
        if (id <= 0) throw new Exception("id is 0.");
        var response = await _dL_Shop.GetShop(id);
        return response;
    }

    public async Task<MessageResponseModel> CreateShop(ShopModel requestModel)
    {
        CheckShopNullValue(requestModel);
        var response = await _dL_Shop.CreateShop(requestModel);
        return response;
    }

    public async Task<MessageResponseModel> UpdateShop(int id, ShopModel requestModel)
    {
        if (id <= 0) throw new Exception("id is 0.");
        CheckShopNullValue(requestModel);
        var response = await _dL_Shop.UpdateShop(id, requestModel);
        return response;
    }

    public async Task<MessageResponseModel> DeleteShop(int id)
    {
        if (id <= 0) throw new Exception("id is 0.");
        var response = await _dL_Shop.DeleteShop(id);
        return response;
    }

    private void CheckShopNullValue(ShopModel shop)
    {
        if (shop == null)
            throw new Exception("shop is null.");

        if (string.IsNullOrWhiteSpace(shop.ShopCode))
            throw new Exception("shop.ShopCode is null.");

        if (string.IsNullOrWhiteSpace(shop.ShopName))
            throw new Exception("shop.ShopName is null.");

        if (string.IsNullOrWhiteSpace(shop.MobileNo))
            throw new Exception("shop.ShopName is null.");

        if (string.IsNullOrWhiteSpace(shop.Address))
            throw new Exception("shop.ShopName is null.");
    }
}