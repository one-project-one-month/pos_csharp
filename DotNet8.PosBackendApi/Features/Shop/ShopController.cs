namespace DotNet8.PosBackendApi.Features.Shop;

[Route("api/v1/shops")]
[ApiController]
public class ShopController : BaseController
{
    private readonly BL_Shop _bL_Shop;
    private readonly ResponseModel _response;

    public ShopController(IServiceProvider serviceProvider, BL_Shop bL_Shop,
        ResponseModel response) : base(serviceProvider)
    {
        _bL_Shop = bL_Shop;
        _response = response;
    }

    [HttpGet]
    public async Task<IActionResult> GetShops()
    {
        try
        {
            var lst = await _bL_Shop.GetShops();
            //var responseModel = _response.ReturnGet
            //    (shopLst.MessageResponse.Message,
            //    shopLst.DataLst.Count,
            //    EnumPos.Shop,
            //    shopLst.MessageResponse.IsSuccess,
            //    shopLst.DataLst);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                Count = lst.DataLst.Count,
                EnumPos = EnumPos.Shop,
                IsSuccess = lst.MessageResponse.IsSuccess,
                Message = lst.MessageResponse.Message,
                Item = lst.DataLst
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShop(int id)
    {
        try
        {
            var shop = await _bL_Shop.GetShop(id);
            //var responseModel = _response.ReturnById
            //    (shop.MessageResponse.Message,
            //    EnumPos.Shop, 
            //    shop.MessageResponse.IsSuccess,
            //    shop.Data);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                EnumPos = EnumPos.Shop,
                IsSuccess = shop.MessageResponse.IsSuccess,
                Message = shop.MessageResponse.Message,
                Item = shop.Data
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateShop(ShopModel shop)
    {
        try
        {
            var model = await _bL_Shop.CreateShop(shop);
            //var responseModel = _response.ReturnCommand
            //    (model.IsSuccess, model.Message,EnumPos.Shop,shop);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                EnumPos = EnumPos.Shop,
                IsSuccess = model.IsSuccess,
                Message = model.Message,
                Item = shop
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        };
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateShop(int id, ShopModel shop)
    {
        try
        {
            var model = await _bL_Shop.UpdateShop(id, shop);
            //var responseModel = _response.ReturnCommand
            //    (model.IsSuccess, model.Message, EnumPos.Shop, shop);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                EnumPos = EnumPos.Shop,
                IsSuccess = model.IsSuccess,
                Message = model.Message,
                Item = shop
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        };
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShop(int id)
    {
        try
        {
            var model = await _bL_Shop.DeleteShop(id);
            //var responseModel = _response.ReturnCommand
            //    (model.IsSuccess, model.Message, EnumPos.Shop);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                EnumPos = EnumPos.Shop,
                IsSuccess = model.IsSuccess,
                Message = model.Message,
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        };
    }

    [HttpGet("{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetShopsWithPagination(int pageNo, int pageSize, [FromQuery] string? search = null)
    {
        try
        {
            var lst = await _bL_Shop.GetShops(pageNo, pageSize, search);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                Count = lst.DataLst.Count,
                EnumPos = EnumPos.Shop,
                IsSuccess = lst.MessageResponse.IsSuccess,
                Message = lst.MessageResponse.Message,
                Item = lst.DataLst,
                PageSetting = lst.PageSetting
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}