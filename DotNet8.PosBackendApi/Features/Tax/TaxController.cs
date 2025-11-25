using DotNet8.PosBackendApi.Models.Setup.Tax;
using System.Collections.Generic;

namespace DotNet8.PosBackendApi.Features.Tax;

[Route("api/v1/taxes")]
[ApiController]
public class TaxController : BaseController
{
    private readonly ResponseModel _response;
    private readonly BL_Tax _bL_Tax;

    public TaxController(ResponseModel response, BL_Tax bL_Tax, IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _response = response;
        _bL_Tax = bL_Tax;
    }

    [HttpGet]
    public async Task<IActionResult> GetTaxList()
    {
        try
        {
            var lst = await _bL_Tax.GetTaxList();
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = lst.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Tax,
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

    [HttpGet("{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetTaxList(int pageNo, int pageSize, [FromQuery] string? search = null)
    {
        try
        {
            var lst = await _bL_Tax.GetTaxList(pageNo, pageSize, search);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = lst.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Tax,
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaxById(int id)
    {
        try
        {
            var item = await _bL_Tax.GetTaxById(id);
            var responseModel = _response.Return
                (new ReturnModel
                {
                    Token = RefreshToken(),
                    IsSuccess = item.MessageResponse.IsSuccess,
                    EnumPos = EnumPos.Tax,
                    Message = item.MessageResponse.Message,
                    Item = item.Data
                });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTax([FromBody] TaxModel requestModel)
    {
        try
        {
            var tax = await _bL_Tax.CreateTax(requestModel);
            var responseModel = _response.Return
                (new ReturnModel
                {
                    Token = RefreshToken(),
                    IsSuccess = tax.IsSuccess,
                    EnumPos = EnumPos.Tax,
                    Message = tax.Message,
                    Item = requestModel
                });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTax([FromBody] TaxModel requestModel, int id)
    {
        try
        {
            var tax = await _bL_Tax.UpdateTax(id, requestModel);
            var responseModel = _response.Return
                (new ReturnModel
                {
                    Token = RefreshToken(),
                    IsSuccess = tax.IsSuccess,
                    EnumPos = EnumPos.Tax,
                    Message = tax.Message,
                    Item = requestModel
                });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTax(int id)
    {
        try
        {
            var tax = await _bL_Tax.DeleteTax(id);
            var responseModel = _response.Return
                (new ReturnModel
                {
                    Token = RefreshToken(),
                    IsSuccess = tax.IsSuccess,
                    EnumPos = EnumPos.Tax,
                    Message = tax.Message,
                    Item = tax
                });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}