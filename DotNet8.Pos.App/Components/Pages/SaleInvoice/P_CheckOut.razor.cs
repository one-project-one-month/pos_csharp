using DotNet8.Pos.App.Models.SaleInvoice;

namespace DotNet8.Pos.App.Components.Pages.SaleInvoice;

public partial class P_CheckOut
{
    [Parameter]
    public List<SaleInvoiceDetailModel> SaleInvoiceDetails { get; set; } = new List<SaleInvoiceDetailModel>();

    private SaleInvoiceModel? reqModel = new SaleInvoiceModel();

    private EnumSaleInvoiceFormType saleInvoiceFormType = EnumSaleInvoiceFormType.Checkout;

    protected override void OnParametersSet()
    {
        reqModel.PaymentAmount = SaleInvoiceDetails.Sum(x => x.Amount);
    }
    private void IncreaseCount(SaleInvoiceDetailModel requestModel)
    {
        requestModel.Quantity += 1;
        SaleInvoiceDetails!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Quantity = requestModel.Quantity; ;
        SaleInvoiceDetails!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Amount = (requestModel.Price * requestModel.Quantity);
    }

    private void DecreaseCount(SaleInvoiceDetailModel requestModel, int quantity)
    {
        if (requestModel.Quantity > 0)
        {
            requestModel.Quantity -= quantity;
            SaleInvoiceDetails!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Quantity = requestModel.Quantity;
            SaleInvoiceDetails!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Amount = (requestModel.Price * requestModel.Quantity);
        }
    }

    private async void Pay()
    {
        reqModel.SaleInvoiceDetails = SaleInvoiceDetails;
        reqModel.SaleInvoiceDateTime = DateTime.Now;
        reqModel.TotalAmount = SaleInvoiceDetails.Sum(x => x.Amount);
        reqModel.StaffCode = "S_001";
        reqModel.PaymentType = "KBZPay";

        var response = await HttpClientService.ExecuteAsync<SaleInvoiceResponseModel>(
            Endpoints.SaleInvoice,
            EnumHttpMethod.Post,
            reqModel
        );
        if (response.IsError)
        {
            InjectService.ShowMessage(response.Message, EnumResponseType.Error);
            saleInvoiceFormType = EnumSaleInvoiceFormType.Checkout;
            return;
        }

        InjectService.ShowMessage(response.Message, EnumResponseType.Success);

        string VoucherNo = response.Data.SaleInvoice.VoucherNo.ToString();
        Nav.NavigateTo($"/sale-receipt/{VoucherNo}");
    }

    private void Back()
    {
        saleInvoiceFormType = EnumSaleInvoiceFormType.SaleProduct;
    }
}