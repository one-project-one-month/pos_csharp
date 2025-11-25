namespace DotNet8.PosBackendApi.Features.Shop;

public class DL_Shop
{
    private readonly AppDbContext _context;

    public DL_Shop(AppDbContext context) => _context = context;

    public async Task<ShopListResponseModel> GetShops()
    {
        var responseModel = new ShopListResponseModel();
        try
        {
            var shopList = await _context
                .TblShops
                .AsNoTracking()
                .ToListAsync();

            responseModel.DataLst = shopList.Select(x => x.Change()).ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.DataLst = new List<ShopModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<ShopResponseModel> GetShop(int id)
    {
        var responseModel = new ShopResponseModel();
        try
        {
            var shop = await _context
                .TblShops
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ShopId == id);
            if (shop is null)
            {
                responseModel.MessageResponse = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            responseModel.Data = shop!.Change();
            responseModel.MessageResponse = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
        }
        catch (Exception ex)
        {
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

    result:
        return responseModel;
    }

    public async Task<MessageResponseModel> CreateShop(ShopModel requestModel)
    {
        var responseModel = new MessageResponseModel();
        try
        {
            var shopCode = await _context.TblShops
            .AsNoTracking()
            .MaxAsync(x => x.ShopCode);
            requestModel.ShopCode = shopCode.GenerateCode(EnumCodePrefix.SP.ToString());

            await _context.TblShops.AddAsync(requestModel.Change());
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

    public async Task<MessageResponseModel> UpdateShop(int id, ShopModel requestModel)
    {
        var responseModel = new MessageResponseModel();
        try
        {
            var shop = await _context.TblShops.AsNoTracking().FirstOrDefaultAsync(x => x.ShopId == id);

            if (shop is null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                return responseModel;
            }

            #region Patch Method Validation Conditions

            if (!string.IsNullOrEmpty(requestModel.ShopCode))
                shop.ShopCode = requestModel.ShopCode;

            if (!string.IsNullOrEmpty(requestModel.ShopName))
                shop.ShopName = requestModel.ShopName;

            if (!string.IsNullOrEmpty(requestModel.Address))
                shop.Address = requestModel.Address;

            if (!string.IsNullOrEmpty(requestModel.MobileNo))
                shop.MobileNo = requestModel.MobileNo;

            #endregion

            _context.Entry(shop).State = EntityState.Modified;
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

    public async Task<MessageResponseModel> DeleteShop(int id)
    {
        var responseModel = new MessageResponseModel();
        try
        {
            var shop = await _context
                .TblShops
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ShopId == id);
            if (shop == null)
            {
                responseModel = new MessageResponseModel(false, EnumStatus.NotFound.ToString());
                goto result;
            }

            _context.Remove(shop);
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

    public async Task<ShopListResponseModel> GetShops(int pageNo, int pageSize, string? search = null)
    {
        var responseModel = new ShopListResponseModel();
        try
        {
            var query = _context
                .TblShops
                .OrderBy(x => x.ShopId)
                .AsNoTracking();

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.ShopCode.Contains(search) ||
                    x.ShopName.Contains(search));
            }

            var shopList = await query
                .Pagination(pageNo, pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();
            var pageCount = totalCount / pageSize;

            if (totalCount % pageSize > 0)
            {
                pageCount++;
            }

            responseModel.DataLst = shopList.Select(x => x.Change()).ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
            responseModel.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
        }
        catch (Exception ex)
        {
            responseModel.DataLst = new List<ShopModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }
}