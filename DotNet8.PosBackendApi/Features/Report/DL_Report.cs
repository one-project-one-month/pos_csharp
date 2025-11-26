using DotNet8.PosBackendApi.Shared;
using DotNet8.PosBackendApi.Models.Setup.Report;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DotNet8.PosBackendApi.Features.Report;

public class DL_Report
{
    private readonly AppDbContext _context;
    private readonly DapperService _dapperService;
    public DL_Report(AppDbContext context, DapperService dapperService)
    {
        _context = context;
        _dapperService = dapperService;
    }

    public async Task<MonthlyReportResponseModel> DailyReport(int dateDay, int dateMonth, int dateYear, int pageNo, int pageSize)
    {
        MonthlyReportResponseModel responseModel = new MonthlyReportResponseModel();
        var query = _context
            .TblSaleInvoices
            .AsNoTracking()
            .Where(x => x.SaleInvoiceDateTime.Day >= dateDay && x.SaleInvoiceDateTime.Month >= dateMonth && x.SaleInvoiceDateTime.Year == dateYear)
            .GroupBy(x => x.SaleInvoiceDateTime.Date)
            .Select(y => new ReportModel
            {
                SaleInvoiceDate = y.First().SaleInvoiceDateTime,
                TotalAmount = y.Sum(c => c.TotalAmount)
            }).OrderBy(x => x.SaleInvoiceDate);
        int totalCount = query.Count();
        int pageCount = totalCount / pageSize;
        if (totalCount % pageSize != 0)
        {
            pageCount = pageCount + 1;
        }
        var report = await query
                .Pagination(pageNo, pageSize)
                .ToListAsync();

        responseModel.Data = report;
        responseModel.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
        responseModel.MessageResponse = responseModel.Data.Count > 0
            ? new MessageResponseModel(true, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.NotFound.ToString());
        return responseModel;
    }

    //public async Task<MonthlyReportResponseModel> DailyReportV1(SaleDailyReportRequestModel requestModel)
    //{
    //    MonthlyReportResponseModel responseModel = new MonthlyReportResponseModel();
    //    var query = _context
    //        .TblSaleInvoices
    //        .AsNoTracking()
    //        .Where(x =>
    //            x.SaleInvoiceDateTime.Day >= requestModel.FromDate.Day &&
    //            x.SaleInvoiceDateTime.Month >= requestModel.FromDate.Month &&
    //            x.SaleInvoiceDateTime.Year == requestModel.FromDate.Year)
    //        .GroupBy(x => x.SaleInvoiceDateTime.Date)
    //        .Select(y => new ReportModel
    //        {
    //            SaleInvoiceDate = y.First().SaleInvoiceDateTime,
    //            TotalAmount = y.Sum(c => c.TotalAmount)
    //        }).OrderBy(x => x.SaleInvoiceDate);

    //    int totalCount = query.Count();
    //    int pageCount = totalCount / requestModel.PageSetting.PageSize;
    //    if (totalCount % requestModel.PageSetting.PageSize != 0)
    //    {
    //        pageCount = pageCount + 1;
    //    }
    //    var report = await query
    //            .Pagination(
    //            requestModel.PageSetting.PageNo,
    //            requestModel.PageSetting.PageSize)
    //            .ToListAsync();

    //    responseModel.Data = report;
    //    responseModel.PageSetting = new PageSettingModel(
    //            requestModel.PageSetting.PageNo,
    //            requestModel.PageSetting.PageSize,
    //            pageCount, totalCount);
    //    responseModel.MessageResponse = responseModel.Data.Count > 0
    //        ? new MessageResponseModel(true, EnumStatus.Success.ToString())
    //        : new MessageResponseModel(false, EnumStatus.NotFound.ToString());
    //    return responseModel;
    //}

    public async Task<MonthlyReportResponseModel> MonthlyReport(int month, int year, int pageNo, int pageSize)
    {
        MonthlyReportResponseModel responseModel = new MonthlyReportResponseModel();
        var query = _context
            .TblSaleInvoices
            .AsNoTracking()
            .Where(x => x.SaleInvoiceDateTime.Month == month && x.SaleInvoiceDateTime.Year == year)
            .GroupBy(x => x.SaleInvoiceDateTime.Date)
            .Select(y => new ReportModel
            {
                SaleInvoiceDate = y.First().SaleInvoiceDateTime,
                TotalAmount = y.Sum(c => c.TotalAmount)
            }).OrderBy(x => x.SaleInvoiceDate);
        int totalCount = query.Count();
        int pageCount = totalCount / pageSize;
        if (totalCount % pageSize != 0)
        {
            pageCount = pageCount + 1;
        }

        var report = await query
                .Pagination(pageNo, pageSize)
                .ToListAsync();

        responseModel.Data = report;
        responseModel.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
        responseModel.MessageResponse = responseModel.Data.Count > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.NotFound.ToString());
        return responseModel;
    }

    public async Task<MonthlyReportResponseModel> YearlyReport(int year, int pageNo, int pageSize)
    {
        MonthlyReportResponseModel responseModel = new MonthlyReportResponseModel();

        var query = _context
            .TblSaleInvoices
            .AsNoTracking()
            .Where(x => x.SaleInvoiceDateTime.Year == year)
            .GroupBy(x => x.SaleInvoiceDateTime.Date)
            .Select(y => new ReportModel
            {
                SaleInvoiceDate = y.First().SaleInvoiceDateTime,
                TotalAmount = y.Sum(c => c.TotalAmount)
            }).OrderBy(x => x.SaleInvoiceDate);
        int totalCount = query.Count();
        int pageCount = totalCount / pageSize;
        if (totalCount % pageSize != 0)
        {
            pageCount = pageCount + 1;
        }
        var report = await query
                .Pagination(pageNo, pageSize)
                .ToListAsync();

        responseModel.Data = report;
        responseModel.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
        responseModel.MessageResponse = responseModel.Data.Count > 0
            ? new MessageResponseModel(true, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.NotFound.ToString());
        return responseModel;
    }

    public async Task<ReportResponseModel> DailyReport(DateTime fromDate, DateTime toDate, int pageNo, int pageSize)
    {
        ReportResponseModel responseModel = new ReportResponseModel();
        var query = _context
            .TblSaleInvoices
            .AsNoTracking()
            .Where(x => x.SaleInvoiceDateTime >= fromDate.AddHours(-12) && x.SaleInvoiceDateTime <= toDate.AddHours(12))
            .GroupBy(x => x.SaleInvoiceDateTime.Date)
            .Select(y => new ReportModel
            {
                SaleInvoiceDate = y.First().SaleInvoiceDateTime,
                TotalAmount = y.Sum(c => c.TotalAmount)
            }).OrderBy(x => x.SaleInvoiceDate);

        int totalCount = query.Count();
        int pageCount = totalCount / pageSize;
        if (totalCount % pageSize != 0)
        {
            pageCount = pageCount + 1;
        }
        var report = await query
                .Pagination(pageNo, pageSize)
                .ToListAsync();

        responseModel.Data = report;
        responseModel.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
        responseModel.MessageResponse = responseModel.Data.Count > 0
            ? new MessageResponseModel(true, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.NotFound.ToString());
        return responseModel;
    }

    public async Task<ReportResponseModel> MonthlyReport(DateTime fromDate, DateTime toDate, int pageNo, int pageSize)
    {
        ReportResponseModel responseModel = new ReportResponseModel();
        //var query = _context
        //    .TblSaleInvoices
        //    .AsNoTracking()
        //    .Where(x => x.SaleInvoiceDateTime >= fromDate.AddHours(-12) && x.SaleInvoiceDateTime <= toDate.AddHours(12))
        //    .GroupBy(x => new { x.SaleInvoiceDateTime.Year, x.SaleInvoiceDateTime.Month })
        //    .Select(y => new ReportModel
        //    {
        //        SaleInvoiceDate = y.First().SaleInvoiceDateTime,
        //        TotalAmount = y.Sum(c => c.TotalAmount)
        //    }).OrderBy(x => x.SaleInvoiceDate);
        //int totalCount = query.Count();
        //int pageCount = totalCount / pageSize;
        //if (totalCount % pageSize != 0)
        //{
        //    pageCount = pageCount + 1;
        //}

        //var report = await query
        //        .Pagination(pageNo, pageSize)
        //        .ToListAsync();

        var parameters = new
        {
            PageNo = pageNo, PageSize = pageSize, FromDate = fromDate.ToString("yyyy-MM-dd"), ToDate = toDate.ToString("yyyy-MM-dd")
        };

        var result = await _dapperService.QueryMultipleAsync<ReportModel, PageSettingModel>("Sp_monthly_report", parameters);

        responseModel.Data = result.Item1.ToList();
        responseModel.PageSetting = result.Item2.FirstOrDefault()!;
        responseModel.MessageResponse = responseModel.Data.Count > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.NotFound.ToString());
        return responseModel;
    }

    public async Task<MonthlyReportResponseModel> YearlyReport(DateTime fromDate, DateTime toDate, int pageNo, int pageSize)
    {
        MonthlyReportResponseModel responseModel = new MonthlyReportResponseModel();

        var query = _context
            .TblSaleInvoices
            .AsNoTracking()
            .Where(x => x.SaleInvoiceDateTime.Year >= fromDate.Year && x.SaleInvoiceDateTime.Year <= toDate.Year)
            .GroupBy(x => x.SaleInvoiceDateTime.Year)
            .Select(y => new ReportModel
            {
                SaleInvoiceDate = y.First().SaleInvoiceDateTime,
                TotalAmount = y.Sum(c => c.TotalAmount)
            }).OrderBy(x => x.SaleInvoiceDate);

        int totalCount = query.Count();
        int pageCount = totalCount / pageSize;
        if (totalCount % pageSize != 0)
        {
            pageCount = pageCount + 1;
        }
        var report = await query
                .Pagination(pageNo, pageSize)
                .ToListAsync();

        responseModel.Data = report;
        responseModel.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
        responseModel.MessageResponse = responseModel.Data.Count > 0
            ? new MessageResponseModel(true, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.NotFound.ToString());
        return responseModel;
    }

    public async Task<BestSellingProductResponseModel> BestSellingProductsReport(DateTime fromDate, DateTime toDate, int pageNo, int pageSize)
    {
        BestSellingProductResponseModel responseModel = new BestSellingProductResponseModel();

        var query = from inv in _context.TblSaleInvoices
                    join det in _context.TblSaleInvoiceDetails on inv.VoucherNo equals det.VoucherNo
                    join prod in _context.TblProducts on det.ProductCode equals prod.ProductCode
                    where inv.SaleInvoiceDateTime >= fromDate && inv.SaleInvoiceDateTime <= toDate
                    group new { det, prod } by new { prod.ProductCode, prod.ProductName } into g
                    select new BestSellingProductModel
                    {
                        ProductCode = g.Key.ProductCode,
                        ProductName = g.Key.ProductName,
                        TotalQuantity = g.Sum(x => x.det.Quantity),
                        TotalAmount = g.Sum(x => x.det.Amount)
                    };

        var orderedQuery = query.OrderByDescending(x => x.TotalQuantity);

        int totalCount = await orderedQuery.CountAsync();
        int pageCount = totalCount / pageSize;
        if (totalCount % pageSize != 0)
        {
            pageCount++;
        }

        var report = await orderedQuery
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        responseModel.Data = report;
        responseModel.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
        responseModel.MessageResponse = responseModel.Data.Count > 0
            ? new MessageResponseModel(true, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.NotFound.ToString());
        return responseModel;
    }

    public async Task<SalesByCategoryResponseModel> SalesByCategoryReport(DateTime fromDate, DateTime toDate, int pageNo, int pageSize)
    {
        SalesByCategoryResponseModel responseModel = new SalesByCategoryResponseModel();

        var query = from inv in _context.TblSaleInvoices
                    join det in _context.TblSaleInvoiceDetails on inv.VoucherNo equals det.VoucherNo
                    join prod in _context.TblProducts on det.ProductCode equals prod.ProductCode
                    join cat in _context.TblProductCategories on prod.ProductCategoryCode equals cat.ProductCategoryCode
                    where inv.SaleInvoiceDateTime >= fromDate && inv.SaleInvoiceDateTime <= toDate
                    group new { det, cat } by new { cat.ProductCategoryCode, cat.ProductCategoryName } into g
                    select new SalesByCategoryModel
                    {
                        ProductCategoryCode = g.Key.ProductCategoryCode,
                        ProductCategoryName = g.Key.ProductCategoryName,
                        TotalAmount = g.Sum(x => x.det.Amount)
                    };

        var orderedQuery = query.OrderByDescending(x => x.TotalAmount);

        int totalCount = await orderedQuery.CountAsync();
        int pageCount = totalCount / pageSize;
        if (totalCount % pageSize != 0)
        {
            pageCount++;
        }

        var report = await orderedQuery
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        responseModel.Data = report;
        responseModel.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
        responseModel.MessageResponse = responseModel.Data.Count > 0
            ? new MessageResponseModel(true, EnumStatus.Success.ToString())
            : new MessageResponseModel(false, EnumStatus.NotFound.ToString());
        return responseModel;
    }
}