namespace DotNet8.PosBackendApi.Features.ProductCategory;

[Route("api/v1/product-categories")]
[ApiController]
public class ProductCategoryController : BaseController
{
    private readonly BL_ProductCategory _productCategory;
    private readonly ResponseModel _response;

    public ProductCategoryController(IServiceProvider serviceProvider, BL_ProductCategory productCategory,
        ResponseModel response) : base(serviceProvider)
    {
        _productCategory = productCategory;
        _response = response;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductCategory()
    {
        try
        {
            var lst = await _productCategory.GetProductCategory();
            //var model = _response.ReturnGet
            //(lst.MessageResponse.Message,
            //    lst.DataList.Count,
            //    EnumPos.ProductCategory,
            //    lst.MessageResponse.IsSuccess,
            //    lst.DataList);

            var model = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                Count = lst.DataList.Count,
                EnumPos = EnumPos.ProductCategory,
                IsSuccess = lst.MessageResponse.IsSuccess,
                Message = lst.MessageResponse.Message,
                Item = lst.DataList
            });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetProductCategory(int pageNo, int pageSize, [FromQuery] string? search = null)
    {
        try
        {
            var item = await _productCategory.GetProductCategory(pageNo, pageSize, search);

            var model = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                EnumPos = EnumPos.ProductCategory,
                IsSuccess = item.MessageResponse.IsSuccess,
                Message = item.MessageResponse.Message,
                Item = item.Data.ProductCategory,
                PageSetting = item.Data.PageSetting
            });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{productCategoryCode}")]
    public async Task<IActionResult> GetProductCategoryByCode(string productCategoryCode)
    {
        try
        {
            var item = await _productCategory.GetProductCategoryByCode(productCategoryCode);
            //var model = _response.ReturnById
            //(item.MessageResponse.Message,
            //    EnumPos.ProductCategory,
            //    item.MessageResponse.IsSuccess,
            //    item.Data);

            var model = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                EnumPos = EnumPos.ProductCategory,
                IsSuccess = item.MessageResponse.IsSuccess,
                Message = item.MessageResponse.Message,
                Item = item.Data
            });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCategoryModel requestModel)
    {
        try
        {
            var item = await _productCategory.CreateProductCategory(requestModel);
            //var model = _response.ReturnCommand
            //    (item.IsSuccess, item.Message, EnumPos.ProductCategory, requestModel);
            var model = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                EnumPos = EnumPos.ProductCategory,
                IsSuccess = item.IsSuccess,
                Message = item.Message,
                Item = requestModel
            });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }

        ;
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, ProductCategoryModel requestModel)
    {
        try
        {
            var item = await _productCategory.UpdateProductCategory(id, requestModel);
            var model = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                EnumPos = EnumPos.ProductCategory,
                IsSuccess = item.IsSuccess,
                Message = item.Message,
                Item = requestModel
            });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var item = await _productCategory.DeleteProductCategory(id);
            //var model = _response.ReturnCommand
            //    (item.IsSuccess, item.Message, EnumPos.ProductCategory);
            var model = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                EnumPos = EnumPos.ProductCategory,
                IsSuccess = item.IsSuccess,
                Message = item.Message,
            });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}