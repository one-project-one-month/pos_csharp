namespace DotNet8.PosBackendApi.Features.SaleInvoice;

[Route("api/v1/sale-invoices")]
[ApiController]
public class SaleInvoiceController : BaseController
{
    private readonly BL_SaleInvoice _saleInvoice;
    private readonly ResponseModel _response;

    public SaleInvoiceController(IServiceProvider serviceProvider, BL_SaleInvoice saleInvoice,
        ResponseModel response) : base(serviceProvider)
    {
        _saleInvoice = saleInvoice;
        _response = response;
    }

    [HttpGet("{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetProductCategory(int pageNo, int pageSize, [FromQuery] string? search = null)
    {
        try
        {
            var item = await _saleInvoice.GetSaleInvoice(pageNo, pageSize, search);

            var model = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                EnumPos = EnumPos.SaleInvoice,
                IsSuccess = item.MessageResponse.IsSuccess,
                Message = item.MessageResponse.Message,
                Item = item.DataList,
                PageSetting = item.Data.PageSetting
            });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetSaleInvoice(DateTime startDate, DateTime endDate)
    {
        try
        {
            var lst = await _saleInvoice.GetSaleInvoice(startDate, endDate);
            var model = _response.Return(
                new ReturnModel
                {
                    Token = RefreshToken(),
                    Count = lst.DataList.Count,
                    EnumPos = EnumPos.SaleInvoice,
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

    [HttpGet("{voucherNo}")]
    public async Task<IActionResult> GetSaleInvoice(string voucherNo)
    {
        try
        {
            var lst = await _saleInvoice.GetSaleInvoice(voucherNo);
            var model = _response.Return(
                new ReturnModel
                {
                    Token = RefreshToken(),
                    EnumPos = EnumPos.SaleInvoice,
                    IsSuccess = lst.MessageResponse.IsSuccess,
                    Message = lst.MessageResponse.Message,
                    Item = lst.Data
                });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateSaleInvoice(SaleInvoiceModel requestModel)
    {
        try
        {
            var responseModel = await _saleInvoice.CreateSaleInvoice(requestModel);
            var model = _response.Return(
                new ReturnModel
                {
                    Token = RefreshToken(),
                    EnumPos = EnumPos.SaleInvoice,
                    IsSuccess = responseModel.MessageResponse.IsSuccess,
                    Message = responseModel.MessageResponse.Message,
                    Item = responseModel.Data
                });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSaleInvoice(int id, SaleInvoiceModel requestModel)
    {
        try
        {
            var item = await _saleInvoice.UpdateSaleInvoice(id, requestModel);
            var model = _response.Return(new ReturnModel
            {
                Token = RefreshToken(),
                EnumPos = EnumPos.SaleInvoice,
                IsSuccess = item.IsSuccess,
                Message = item.Message,
                Item = item
            });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSaleInvoice(int id)
    {
        try
        {
            var item = await _saleInvoice.DeleteSaleInvoice(id);
            var model = _response.Return(new ReturnModel
            {
                Token = RefreshToken(),
                EnumPos = EnumPos.SaleInvoice,
                IsSuccess = item.IsSuccess,
                Message = item.Message,
                Item = item
            });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}