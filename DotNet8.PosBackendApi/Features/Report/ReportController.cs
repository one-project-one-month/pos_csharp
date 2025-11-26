using DotNet8.PosBackendApi.Shared;
using DotNet8.PosBackendApi.Models.Setup.Report;

namespace DotNet8.PosBackendApi.Features.Report;

[Route("api/v1/report")]
[ApiController]
public class ReportController : BaseController
{
    private readonly BL_Report _report;
    private readonly ResponseModel _response;

    public ReportController(IServiceProvider serviceProvider, BL_Report report, ResponseModel response)
        : base(serviceProvider)
    {
        _report = report;
        _response = response;
    }

    [HttpGet("daily-report/{dateDay}/{dateMonth}/{dateYear}/{pageNo}/{pageSize}")]
    public async Task<IActionResult> DailyReport(int dateDay, int dateMonth, int dateYear, int pageNo, int pageSize)
    {
        try
        {
            var lst = await _report.DailyReport(dateDay, dateMonth, dateYear, pageNo, pageSize);
            var model = _response.Return(
                new ReturnModel
                {
                    Token = RefreshToken(),
                    Count = lst.Data.Count,
                    EnumPos = EnumPos.Report,
                    IsSuccess = lst.MessageResponse.IsSuccess,
                    Message = lst.MessageResponse.Message,
                    Item = lst.Data,
                    PageSetting = lst.PageSetting
                });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    //[HttpPost("daily-report")]
    //public async Task<IActionResult> DailyReportV1(SaleDailyReportRequestModel requestModel)
    //{
    //    try
    //    {
    //        var lst = await _report.DailyReportV1(requestModel);
    //        var model = _response.Return(
    //            new ReturnModel
    //            {
    //                Token = RefreshToken(),
    //                Count = lst.Data.Count,
    //                EnumPos = EnumPos.Report,
    //                IsSuccess = lst.MessageResponse.IsSuccess,
    //                Message = lst.MessageResponse.Message,
    //                Item = lst.Data,
    //                PageSetting = lst.PageSetting
    //            });
    //        return Content(model);
    //    }
    //    catch (Exception ex)
    //    {
    //        return InternalServerError(ex);
    //    }
    //}

    //[Route("monthly-report")]
    /*[HttpGet("monthly-report/{month}/{year}/{pageNo}/{pageSize}")]
    public async Task<IActionResult> MonthlyReport(int month, int year, int pageNo, int pageSize)
    {
        try
        {
            var lst = await _report.MonthlyReport(month, year, pageNo, pageSize);
            var model = _response.Return(
                new ReturnModel
                {
                    Token = RefreshToken(),
                    Count = lst.Data.Count,
                    EnumPos = EnumPos.Report,
                    IsSuccess = lst.MessageResponse.IsSuccess,
                    Message = lst.MessageResponse.Message,
                    Item = lst.Data,
                    PageSetting = lst.PageSetting
                });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }*/

    //[Route("yearly-report")]

    [HttpGet("yearly-report/{year}/{pageNo}/{pageSize}")]
    public async Task<IActionResult> YearlyReport(int year, int pageNo, int pageSize)
    {
        try
        {
            var lst = await _report.YearlyReport(year, pageNo, pageSize);
            var model = _response.Return(
                new ReturnModel
                {
                    Token = RefreshToken(),
                    Count = lst.Data.Count,
                    EnumPos = EnumPos.Report,
                    IsSuccess = lst.MessageResponse.IsSuccess,
                    Message = lst.MessageResponse.Message,
                    Item = lst.Data,
                    PageSetting = lst.PageSetting
                });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("daily-report/{fromDate}/{toDate}/{pageNo}/{pageSize}")]
    public async Task<IActionResult> DailyReport(DateTime fromDate, DateTime toDate, int pageNo, int pageSize)
    {
        try
        {
            var lst = await _report.DailyReport(fromDate, toDate, pageNo, pageSize);
            var model = _response.Return(
                new ReturnModel
                {
                    Token = RefreshToken(),
                    Count = lst.Data.Count,
                    EnumPos = EnumPos.Report,
                    IsSuccess = lst.MessageResponse.IsSuccess,
                    Message = lst.MessageResponse.Message,
                    Item = lst.Data,
                    PageSetting = lst.PageSetting
                });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("monthly-report/{fromDate}/{toDate}/{pageNo}/{pageSize}")]
    public async Task<IActionResult> MonthlyReport(DateTime fromDate, DateTime toDate, int pageNo, int pageSize)
    {
        try
        {
            var lst = await _report.MonthlyReport(fromDate, toDate, pageNo, pageSize);
            var model = _response.Return(
                new ReturnModel
                {
                    Token = RefreshToken(),
                    Count = lst.Data.Count,
                    EnumPos = EnumPos.Report,
                    IsSuccess = lst.MessageResponse.IsSuccess,
                    Message = lst.MessageResponse.Message,
                    Item = lst.Data,
                    PageSetting = lst.PageSetting
                });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("yearly-report/{fromDate}/{toDate}/{pageNo}/{pageSize}")]
    public async Task<IActionResult> YearlyReport(DateTime fromDate, DateTime toDate, int pageNo, int pageSize)
    {
        try
        {
            var lst = await _report.YearlyReport(fromDate, toDate, pageNo, pageSize);
            var model = _response.Return(
                new ReturnModel
                {
                    Token = RefreshToken(),
                    Count = lst.Data.Count,
                    EnumPos = EnumPos.Report,
                    IsSuccess = lst.MessageResponse.IsSuccess,
                    Message = lst.MessageResponse.Message,
                    Item = lst.Data,
                    PageSetting = lst.PageSetting
                });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("best-selling-products/{fromDate}/{toDate}/{pageNo}/{pageSize}")]
    public async Task<IActionResult> BestSellingProductsReport(DateTime fromDate, DateTime toDate, int pageNo, int pageSize)
    {
        try
        {
            var lst = await _report.BestSellingProductsReport(fromDate, toDate, pageNo, pageSize);
            var model = _response.Return(
                new ReturnModel
                {
                    Token = RefreshToken(),
                    Count = lst.Data.Count,
                    EnumPos = EnumPos.Report,
                    IsSuccess = lst.MessageResponse.IsSuccess,
                    Message = lst.MessageResponse.Message,
                    Item = lst.Data,
                    PageSetting = lst.PageSetting
                });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpGet("sales-by-category/{fromDate}/{toDate}/{pageNo}/{pageSize}")]
    public async Task<IActionResult> SalesByCategoryReport(DateTime fromDate, DateTime toDate, int pageNo, int pageSize)
    {
        try
        {
            var lst = await _report.SalesByCategoryReport(fromDate, toDate, pageNo, pageSize);
            var model = _response.Return(
                new ReturnModel
                {
                    Token = RefreshToken(),
                    Count = lst.Data.Count,
                    EnumPos = EnumPos.Report,
                    IsSuccess = lst.MessageResponse.IsSuccess,
                    Message = lst.MessageResponse.Message,
                    Item = lst.Data,
                    PageSetting = lst.PageSetting
                });
            return Content(model);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}