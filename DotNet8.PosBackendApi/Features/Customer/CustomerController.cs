namespace DotNet8.PosBackendApi.Features.Customer;

[Route("api/v1/customers")]
[ApiController]
public class CustomerController : BaseController
{
    private readonly BL_Customer _bL_Customer;
    private readonly ResponseModel _response;

    public CustomerController(
        IServiceProvider serviceProvider,
        BL_Customer bL_Customer,
        ResponseModel response) : base(serviceProvider)
    {
        _bL_Customer = bL_Customer;
        _response = response;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomer()
    {
        try
        {
            var customerLst = await _bL_Customer.GetCustomer();
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                Count = customerLst.DataLst.Count,
                IsSuccess = customerLst.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Customer,
                Message = customerLst.MessageResponse.Message,
                Item = customerLst.DataLst
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetCustomer(int pageNo, int pageSize, [FromQuery] string? search = null)
    {
        try
        {
            var customerLst = await _bL_Customer.GetCustomer(pageNo, pageSize, search);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                Count = customerLst.DataLst.Count,
                IsSuccess = customerLst.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Customer,
                Message = customerLst.MessageResponse.Message,
                Item = customerLst.DataLst,
                PageSetting = customerLst.PageSetting
            });

            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("{customerCode}")]
    public async Task<IActionResult> GetCustomerByCode(string customerCode)
    {
        try
        {
            var customer = await _bL_Customer.GetCustomerByCode(customerCode);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = customer.MessageResponse.IsSuccess,
                EnumPos = EnumPos.Customer,
                Message = customer.MessageResponse.Message,
                Item = customer.Data
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CustomerModel requestModel)
    {
        try
        {
            var customer = await _bL_Customer.CreateCustomer(requestModel);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = customer.IsSuccess,
                EnumPos = EnumPos.Customer,
                Message = customer.Message,
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
    public async Task<IActionResult> UpdateCustomer(int id, CustomerModel requestModel)
    {
        try
        {
            var customer = await _bL_Customer.UpdateCustomer(id, requestModel);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = customer.IsSuccess,
                EnumPos = EnumPos.Customer,
                Message = customer.Message,
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
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        try
        {
            var customer = await _bL_Customer.DeleteCustomer(id);
            var responseModel = _response.Return
            (new ReturnModel
            {
                Token = RefreshToken(),
                IsSuccess = customer.IsSuccess,
                EnumPos = EnumPos.Customer,
                Message = customer.Message,
            });
            return Content(responseModel);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}