namespace DotNet8.PosBackendApi.Features.SaleInvoice;

public class DL_SaleInvoice
{
    private readonly AppDbContext _context;
    private readonly DapperService _dapperService;

    public DL_SaleInvoice(AppDbContext context, DapperService dapperService)
    {
        _context = context;
        _dapperService = dapperService;
    }

    //public async Task<SaleInvoiceListResponseModel> GetSaleInvoice()
    //{
    //    var responseModel = new SaleInvoiceListResponseModel();
    //    try
    //    {
    //        var lst = await _context
    //            .TblSaleInvoices
    //            .AsNoTracking()
    //            .ToListAsync();
    //        if (lst == null)
    //        {
    //            responseModel.MessageResponse = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
    //            goto result;
    //        }
    //        foreach (var item in lst)
    //        {
    //            SaleInvoiceModel saleInvoiceModel = new SaleInvoiceModel();
    //            saleInvoiceModel = item.Change();
    //            var detailList = await _context
    //                .TblSaleInvoiceDetails
    //                .AsNoTracking()
    //                .Where(x => x.VoucherNo == item.VoucherNo)
    //                .ToListAsync();
    //            saleInvoiceModel.SaleInvoiceDetails = detailList.Select(x => x.Change()).ToList();
    //            responseModel.DataList.Add(saleInvoiceModel);
    //        }
    //        responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
    //    }
    //    catch (Exception ex)
    //    {
    //        responseModel.DataList = new List<SaleInvoiceModel>();
    //        responseModel.MessageResponse = new MessageResponseModel(false, ex);
    //    }
    //    result:
    //    return responseModel;
    //}

    public async Task<SaleInvoiceListResponseModel> GetSaleInvoice(int pageNo, int pageSize, string? search = null)
    {
        var responseModel = new SaleInvoiceListResponseModel();
        try
        {
            var query = _context
                .TblSaleInvoices
                .AsNoTracking();

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.VoucherNo.Contains(search) ||
                    x.StaffCode.Contains(search));
            }

            var totalCount = await query.CountAsync();
            var pageCount = totalCount / pageSize;
            if (totalCount % pageSize > 0)
                pageCount++;

            var lst = await query
                .Pagination(pageNo, pageSize)
                .ToListAsync();

            foreach (var item in lst)
            {
                SaleInvoiceModel saleInvoiceModel = new SaleInvoiceModel();
                saleInvoiceModel = item.Change();
                var detailList = await _context
                    .TblSaleInvoiceDetails
                    .AsNoTracking()
                    .Where(x => x.VoucherNo == item.VoucherNo)
                    .ToListAsync();
                saleInvoiceModel.SaleInvoiceDetails = detailList.Select(x => x.Change()).ToList();
                responseModel.DataList.Add(saleInvoiceModel);
            }

            responseModel.Data = new SaleInvoiceDataModel
            {
                SaleInvoice = responseModel.DataList,
                PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount)
            };
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.DataList = new List<SaleInvoiceModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<SaleInvoiceListResponseModel> GetSaleInvoice(DateTime startDate, DateTime endDate)
    {
        var responseModel = new SaleInvoiceListResponseModel();
        try
        {
            #region Get all sale invoice records

            var lstSaleInvoice = await _context
                .TblSaleInvoices
                .AsNoTracking()
                .Where(x => x.SaleInvoiceDateTime.Date >= startDate.Date && x.SaleInvoiceDateTime.Date <= endDate.Date)
                .ToListAsync();
            if (lstSaleInvoice is null)
            {
                responseModel.MessageResponse = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            #endregion

            #region Get all sale invoice detail records

            var lstVoucherNo = lstSaleInvoice.Select(x => x.VoucherNo).ToArray();
            var lstSaleInvoiceDetail = await _context.TblSaleInvoiceDetails.AsNoTracking()
                .Where(x => lstVoucherNo.Contains(x.VoucherNo)).ToListAsync();
            if (lstSaleInvoiceDetail is null)
            {
                responseModel.MessageResponse = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            #endregion

            #region Prepare response model

            List<SaleInvoiceModel> saleInvoiceModels = lstSaleInvoice.Change(lstSaleInvoiceDetail);
            responseModel.DataList = saleInvoiceModels;
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());

            #endregion
        }
        catch (Exception ex)
        {
            responseModel.DataList = new List<SaleInvoiceModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        result:
        return responseModel;
    }

    public async Task<SaleInvoiceResponseModel> GetSaleInvoice(string voucherNo)
    {
        var responseModel = new SaleInvoiceResponseModel();
        try
        {
            var item = await _context
                .TblSaleInvoices
                .AsNoTracking()
                .Where(x => x.VoucherNo == voucherNo)
                .FirstOrDefaultAsync();
            if (item is null)
            {
                responseModel.MessageResponse = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            responseModel.Data = item.Change();
            var detailList = await _context
                .TblSaleInvoiceDetails
                .AsNoTracking()
                .Where(x => x.VoucherNo == item.VoucherNo)
                .ToListAsync();
            responseModel.Data.SaleInvoiceDetails = detailList.Select(x => x.Change()).ToList();

            // Bind Product Info
            foreach (var detail in responseModel.Data.SaleInvoiceDetails)
            {
                var pItem = await _context.TblProducts
                    .AsNoTracking()
                    .Where(x => x.ProductCode == detail.ProductCode)
                    .FirstOrDefaultAsync();
                detail.ProductName = pItem!.ProductName;
            }

            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.Data = new SaleInvoiceModel();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        result:
        return responseModel;
    }

    #region info

    // 1, 00001
    // 2, 00002
    // 3, 00003

    // 5
    // 00005
    // 00006

    // Id, Code, Letter, TotalNumber, Sequence
    // 1, SaleInvoice, VC_, 8, 4
    // VC_00000002

    #endregion

    public async Task<SaleInvoiceResponseModel> CreateSaleInvoice(SaleInvoiceModel model)
    {
        SaleInvoiceResponseModel responseModel = new SaleInvoiceResponseModel();
        try
        {
            // Generate Voucher Code
            //string voucherNo = $"VC_{DateTime.Now.ToString("yyyymmddhhmmssfff")}";
            string voucherNo = _dapperService.QueryStoredProcedure<string>("Sp_GenerateSaleInvoiceNo");
            model.VoucherNo = voucherNo;

            // Tax Calculation
            decimal taxAmount = await TaxCalculation(model.TotalAmount);
            model.Tax = taxAmount;

            // Bind Product Info
            foreach (var item in model.SaleInvoiceDetails)
            {
                var pItem = await _context.TblProducts
                    .AsNoTracking()
                    .Where(x => x.ProductCode == item.ProductCode)
                    .FirstOrDefaultAsync();
                item.ProductName = pItem!.ProductName;
            }

            await _context.TblSaleInvoices.AddAsync(model.Change());
            await _context.TblSaleInvoiceDetails
                .AddRangeAsync(model.SaleInvoiceDetails!.Select(x => x.Change(voucherNo)).ToList());
            var result = await _context.SaveChangesAsync();
            responseModel.Data = result > 0 ? model : new SaleInvoiceModel();

            responseModel.MessageResponse = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel.Data = new SaleInvoiceModel();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<MessageResponseModel> UpdateSaleInvoice(int id, SaleInvoiceModel requestModel)
    {
        var responseModel = new MessageResponseModel();
        try
        {
            var item = await _context.TblSaleInvoices.AsNoTracking().FirstOrDefaultAsync(x => x.SaleInvoiceId == id);

            if (item is null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                return responseModel;
            }

            #region Patch Method Validation Codition

            if (!string.IsNullOrEmpty(requestModel.CustomerCode))
                item.CustomerCode = requestModel.CustomerCode;

            if(!string.IsNullOrEmpty(requestModel.CustomerAccountNo))
                item.CustomerAccountNo = requestModel.CustomerAccountNo;    

            if (!string.IsNullOrEmpty(requestModel.VoucherNo))
                item.VoucherNo = requestModel.VoucherNo;

            if (!string.IsNullOrEmpty(requestModel.PaymentType))
                item.PaymentType = requestModel.PaymentType;

            if (!string.IsNullOrEmpty(requestModel.StaffCode))
                item.StaffCode = requestModel.StaffCode;

            if (requestModel.SaleInvoiceDateTime != null)
                item.SaleInvoiceDateTime = requestModel.SaleInvoiceDateTime;

            if (requestModel.PaymentAmount > 0)
                item.PaymentAmount = requestModel.PaymentAmount;

            if (requestModel.ReceiveAmount > 0)
                item.ReceiveAmount = requestModel.ReceiveAmount;

            if (requestModel.TotalAmount > 0)
                item.TotalAmount = requestModel.TotalAmount;

            if (requestModel.Change > 0)
                item.Change = requestModel.Change;

            if (requestModel.Tax > 0)
                item.Tax = requestModel.Tax;

            if (requestModel.Discount > 0)
                item.Discount = requestModel.Discount;

            #endregion

            _context.TblSaleInvoices.Update(item);
            var result = await _context.SaveChangesAsync();

            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<MessageResponseModel> DeleteSaleInvoice(int id)
    {
        MessageResponseModel responseModel = new MessageResponseModel();
        try
        {
            var item = await _context
                .TblSaleInvoices
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SaleInvoiceId == id);
            if (item is null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            var lstDetail = await _context
                .TblSaleInvoiceDetails
                .AsNoTracking()
                .Where(x => x.VoucherNo == item.VoucherNo)
                .ToListAsync();

            if (lstDetail is not null)
                _context.TblSaleInvoiceDetails.RemoveRange(lstDetail);

            _context.TblSaleInvoices.Remove(item);
            _context.Entry(item).State = EntityState.Deleted;
            var result = await _context.SaveChangesAsync();
            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }

        result:
        return responseModel;
    }

    private async Task<decimal> TaxCalculation(decimal amount)
    {
        decimal result = 0;
        decimal balance = amount;
        while (balance != 0)
        {
            var lstTax = await _context.Tbl_Taxes
                .AsNoTracking()
                .Where(x => (balance > x.ToAmount) || (balance >= x.FromAmount && balance <= x.ToAmount))
                .OrderBy(x => x.FromAmount)
                .ToListAsync();
            if (lstTax == null || lstTax.Count == 0) return result;
            foreach (var item in lstTax)
            {
                var amountRange = item.ToAmount - item.FromAmount;

                if (balance > amountRange)
                {
                    result += Convert.ToDecimal((amountRange * (item.Percentage / 100)) + item.FixedAmount);
                    balance -= (item.ToAmount - item.FromAmount);
                }
                else
                {
                    result += Convert.ToDecimal((balance * (item.Percentage / 100)) + item.FixedAmount);
                    balance -= balance;
                }
            }
        }
        return result;
    }
}