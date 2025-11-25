namespace DotNet8.PosBackendApi.Features.SaleInvoice;

public class BL_SaleInvoice
{
    private readonly DL_SaleInvoice _saleInvoice;

    public BL_SaleInvoice(DL_SaleInvoice saleInvoice) => _saleInvoice = saleInvoice;

    public async Task<SaleInvoiceListResponseModel> GetSaleInvoice(int pageNo, int pageSize, string? search = null)
    {
        return await _saleInvoice.GetSaleInvoice(pageNo, pageSize, search);
    }

    public async Task<SaleInvoiceListResponseModel> GetSaleInvoice(
        DateTime startDate,
        DateTime endDate
    )
    {
        var response = await _saleInvoice.GetSaleInvoice(startDate, endDate);
        return response;
    }

    public async Task<SaleInvoiceResponseModel> GetSaleInvoice(string voucherNo)
    {
        var response = await _saleInvoice.GetSaleInvoice(voucherNo);
        return response;
    }

    public async Task<SaleInvoiceResponseModel> CreateSaleInvoice(SaleInvoiceModel saleInvoice)
    {
        CheckSaleInvoiceModel(saleInvoice);
        var response = await _saleInvoice.CreateSaleInvoice(saleInvoice);
        return response;
    }

    public async Task<MessageResponseModel> UpdateSaleInvoice(int id, SaleInvoiceModel saleInvoice)
    {
        if (id <= 0)
            throw new Exception("SaleInvoiceId is null");
        var response = await _saleInvoice.UpdateSaleInvoice(id, saleInvoice);
        return response;
    }

    public async Task<MessageResponseModel> DeleteSaleInvoice(int id)
    {
        if (id <= 0)
            throw new Exception("SaleInvoiceId is null");
        var response = await _saleInvoice.DeleteSaleInvoice(id);
        return response;
    }

    private static void CheckSaleInvoiceModel(SaleInvoiceModel saleInvoice)
    {
        if (saleInvoice == null)
            throw new Exception("SaleInvoice is null.");

        //if (!saleInvoice.SaleInvoiceDateTime)
        //    throw new Exception("SaleInvoiceDateTime is null");

        if (string.IsNullOrEmpty(saleInvoice.VoucherNo))
            throw new Exception("VoucherNo is null");

        if (saleInvoice.TotalAmount is 0)
            throw new Exception("TotalAmount is not null");

        if (saleInvoice.Discount < 0)
            throw new Exception("Discount is invalid");

        if (string.IsNullOrEmpty(saleInvoice.StaffCode))
            throw new Exception("StaffCode is null");

        if (saleInvoice.Tax < 0)
            throw new Exception("Tax is invalid");

        if (string.IsNullOrEmpty(saleInvoice.PaymentType))
            throw new Exception("PaymentType is null");

        if (string.IsNullOrEmpty(saleInvoice.CustomerAccountNo))
            throw new Exception("CustomerAccountNo is Null");

        if (saleInvoice.PaymentAmount is 0)
            throw new Exception("PaymentAmount is 0");

        if (saleInvoice.ReceiveAmount is 0)
            throw new Exception("ReceiveAmount is 0");

        if (saleInvoice.Change < 0)
            throw new Exception("Change amount is invalid");

        if (string.IsNullOrEmpty(saleInvoice.CustomerCode))
            throw new Exception("Customer code is null");
    }
}
