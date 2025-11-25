using DotNet8.Pos.App.Models.SaleInvoice;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DotNet8.Pos.App.Components.Pages.SaleInvoice;

public partial class P_CheckOut
{
    [Parameter]
    public List<SaleInvoiceDetailModel> SaleInvoiceDetails { get; set; } = new List<SaleInvoiceDetailModel>();

    private SaleInvoiceModel? reqModel = new SaleInvoiceModel();

    private EnumSaleInvoiceFormType saleInvoiceFormType = EnumSaleInvoiceFormType.Checkout;

    private string selectedPaymentMethod = "KBZPay";

    protected override void OnParametersSet()
    {
        reqModel.PaymentAmount = SaleInvoiceDetails.Sum(x => x.Amount);
        if (string.IsNullOrWhiteSpace(reqModel.CustomerCode))
        {
            reqModel.CustomerCode = "C_001"; // Default customer code
        }
        CalculateChange();
    }

    private void CalculateChange()
    {
        // Change is a computed property (ReceiveAmount - PaymentAmount)
        // No need to set it, just refresh the UI
        StateHasChanged();
    }

    private void OnReceiveAmountChanged(decimal? value)
    {
        reqModel.ReceiveAmount = value ?? 0;
        CalculateChange();
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
        // Validate SaleInvoiceDetails
        if (SaleInvoiceDetails == null || SaleInvoiceDetails.Count == 0 || SaleInvoiceDetails.All(x => x.Quantity <= 0))
        {
            InjectService.ShowMessage("Please add at least one item to checkout.", EnumResponseType.Error);
            return;
        }

        // Validate TotalAmount
        var totalAmount = SaleInvoiceDetails.Sum(x => x.Amount);
        if (totalAmount <= 0)
        {
            InjectService.ShowMessage("Total amount must be greater than zero.", EnumResponseType.Error);
            return;
        }

        // Validate PaymentType
        if (string.IsNullOrWhiteSpace(selectedPaymentMethod))
        {
            InjectService.ShowMessage("Please select a payment method.", EnumResponseType.Error);
            return;
        }

        // Validate CustomerAccountNo (required by backend)
        if (string.IsNullOrWhiteSpace(reqModel.CustomerAccountNo))
        {
            InjectService.ShowMessage("Customer Account No is required.", EnumResponseType.Error);
            return;
        }

        // Validate ReceiveAmount
        if (reqModel.ReceiveAmount == null || reqModel.ReceiveAmount <= 0)
        {
            InjectService.ShowMessage("Receive Amount must be greater than zero.", EnumResponseType.Error);
            return;
        }

        // Validate PaymentAmount
        if (reqModel.PaymentAmount == null || reqModel.PaymentAmount <= 0)
        {
            InjectService.ShowMessage("Payment Amount must be greater than zero.", EnumResponseType.Error);
            return;
        }

        // Validate Change (should not be negative)
        var change = (reqModel.ReceiveAmount ?? 0) - (reqModel.PaymentAmount ?? 0);
        if (change < 0)
        {
            InjectService.ShowMessage("Receive Amount must be greater than or equal to Payment Amount.", EnumResponseType.Error);
            return;
        }

        // Validate CustomerCode (required by backend)
        if (string.IsNullOrWhiteSpace(reqModel.CustomerCode))
        {
            // Set default customer code if not provided
            reqModel.CustomerCode = "C_001";
        }

        // Set model properties
        reqModel.SaleInvoiceDetails = SaleInvoiceDetails;
        reqModel.SaleInvoiceDateTime = DateTime.Now;
        reqModel.TotalAmount = totalAmount;
        reqModel.StaffCode = "S_001";
        reqModel.PaymentType = selectedPaymentMethod;
        reqModel.Discount = reqModel.Discount < 0 ? 0 : reqModel.Discount;
        reqModel.Tax = reqModel.Tax < 0 ? 0 : reqModel.Tax;
        //reqModel.Change = change;

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

        string VoucherNo = response.Data.SaleInvoice.VoucherNo?.ToString()!;
        
        // Show receipt as dialog
        var parameters = new DialogParameters<P_SaleReceipt>
        {
            { x => x.VoucherNo, VoucherNo }
        };
        
        var options = new MudBlazor.DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            DisableBackdropClick = true,
            CloseOnEscapeKey = false
        };
        
        var dialog = await DialogService.ShowAsync<P_SaleReceipt>("Receipt", parameters, options);
        await dialog.Result;
    }

    private void Back()
    {
        saleInvoiceFormType = EnumSaleInvoiceFormType.SaleProduct;
    }
}