using DotNet8.PosBackendApi.Models.Setup.Tax;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;

namespace DotNet8.PosBackendApi.Features.Tax;

public class DL_Tax
{
    private readonly AppDbContext _context;

    public DL_Tax(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TaxListResponseModel> GetTaxList()
    {
        TaxListResponseModel taxListResponseModel = new();
        try
        {
            var lst = await _context.Tbl_Taxes
                .AsNoTracking()
                .OrderByDescending(x => x.TaxId)
                .ToListAsync();

            taxListResponseModel.DataLst = lst.Select(x => x.Change()).ToList();
            taxListResponseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            taxListResponseModel.DataLst = new List<TaxModel>();
            taxListResponseModel.MessageResponse = new MessageResponseModel(false, ex);
        }
        return taxListResponseModel;
    }

    public async Task<TaxListResponseModel> GetTaxList(int pageNo, int pageSize, string? search = null)
    {
        TaxListResponseModel taxListResponseModel = new();
        try
        {
            var query = _context
                .Tbl_Taxes
                .OrderByDescending(x => x.TaxId)
                .AsNoTracking();

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.FromAmount.ToString().Contains(search) ||
                    x.ToAmount.ToString().Contains(search) ||
                    x.Percentage.ToString().Contains(search));
            }

            var tax = await query
                .Pagination(pageNo, pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();
            var pageCount = totalCount / pageSize;

            if (totalCount % pageSize > 0)
            {
                pageCount++;
            }

            taxListResponseModel.DataLst = tax.Select(x => x.Change()).ToList();
            taxListResponseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
            taxListResponseModel.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
        }
        catch (Exception ex)
        {
            taxListResponseModel.DataLst = new List<TaxModel>();
            taxListResponseModel.MessageResponse = new MessageResponseModel(false, ex);
        }
        return taxListResponseModel;
    }

    public async Task<TaxResponseModel> GetTaxById(int id)
    {
        TaxResponseModel responseModel = new();
        try
        {
            var item = await _context.Tbl_Taxes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TaxId == id);
            if (item is null)
            {
                responseModel.MessageResponse = new MessageResponseModel
                    (false, EnumStatus.NotFound.ToString());
                goto result;
            }

            responseModel.Data = item.Change();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.Data = new TaxModel();
            responseModel.MessageResponse = new MessageResponseModel(false, ex.Message);
        }
    result:
        return responseModel;
    }

    public async Task<MessageResponseModel> CreateTax(TaxModel requestModel)
    {
        var responseModel = new MessageResponseModel();
        try
        {
            await _context.Tbl_Taxes.AddAsync(requestModel.Change());
            int result = await _context.SaveChangesAsync();
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

    public async Task<MessageResponseModel> UpdateTax(int id, TaxModel requestModel)
    {
        var responseModel = new MessageResponseModel();
        try
        {
            var item = await _context.Tbl_Taxes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TaxId == id);
            if (item is null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                return responseModel;
            }

            #region Patch

            if (requestModel.FromAmount != 0)
                item.FromAmount = requestModel.FromAmount;

            if (requestModel.ToAmount != 0)
                item.ToAmount = requestModel.ToAmount;

            if (requestModel.Percentage != 0)
            {
                item.Percentage = requestModel.Percentage;
                item.FixedAmount = default;
            }

            if (requestModel.FixedAmount != 0)
            {
                item.FixedAmount = requestModel.FixedAmount;
                item.Percentage = default;
            }

            if (!string.IsNullOrEmpty(requestModel.TaxType))
                item.TaxType = requestModel.TaxType;

            _context.Entry(item).State = EntityState.Modified;
            int result = await _context.SaveChangesAsync();
            responseModel = result > 0
                ? new MessageResponseModel(true, EnumStatus.Success.ToString())
                : new MessageResponseModel(false, EnumStatus.Fail.ToString());

            #endregion

        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex);
        }
        return responseModel;
    }

    public async Task<MessageResponseModel> DeleteTax(int id)
    {
        MessageResponseModel responseModel = new();
        try
        {
            var item = await _context.Tbl_Taxes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TaxId == id);
            if (item is null)
            {
                responseModel = new MessageResponseModel(false, nameof(EnumStatus.NotFound));
                return responseModel;
            }

            _context.Tbl_Taxes.Remove(item);
            int result = await _context.SaveChangesAsync();

            responseModel = result > 0
                ? new MessageResponseModel(true, nameof(EnumStatus.Success))
                : new MessageResponseModel(false, nameof(EnumStatus.Fail));

            return responseModel;
        }
        catch (Exception ex)
        {
            responseModel = new MessageResponseModel(false, ex.Message);
        }
        return responseModel;
    }
}