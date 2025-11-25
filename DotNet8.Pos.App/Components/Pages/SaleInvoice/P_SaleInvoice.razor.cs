using DotNet8.Pos.App.Models.SaleInvoice;
using Newtonsoft.Json;

namespace DotNet8.Pos.App.Components.Pages.SaleInvoice;

public partial class P_SaleInvoice
{
    private SaleInvoiceModel? reqModel = new SaleInvoiceModel();
    private ProductListResponseModel? ResponseModel;
    List<ProductModel> lstProduct;
    private List<SaleInvoiceDetailModel>? lstSaleInvoice = new List<SaleInvoiceDetailModel>();
    private SaleInvoiceModel saleInvoiceModel = new SaleInvoiceModel();
    private EnumSaleInvoiceFormType saleInvoiceFormType = EnumSaleInvoiceFormType.SaleProduct;


    public string Search { get; set; }

    protected override async void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            await InjectService.EnableLoading();
            await List();
            StateHasChanged();
            await InjectService.DisableLoading();
        }
    }

    private async Task List()
    {
        ResponseModel = await HttpClientService.ExecuteAsync<ProductListResponseModel>(Endpoints.Product, EnumHttpMethod.Get);
        lstProduct = ResponseModel.Data.Product;
    }

    private void AddItem(ProductModel requestModel)
    {
        SaleInvoiceDetailModel saleInvoiceDetail = new SaleInvoiceDetailModel
        {
            ProductCode = requestModel.ProductCode,
            ProductName = requestModel.ProductName,
            Price = requestModel.Price,
        };

        if (!lstSaleInvoice.Where(x => x.ProductCode == requestModel.ProductCode).Any())
        {
            saleInvoiceDetail.Quantity = 1;
            saleInvoiceDetail.Amount = requestModel.Price;
            lstSaleInvoice!.Add(saleInvoiceDetail);
        }
        else
        {
            lstSaleInvoice.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Quantity += 1;
            lstSaleInvoice.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Amount += requestModel.Price;
        }
        Console.WriteLine(lstSaleInvoice.Select(x => x.Price * x.Quantity).Sum());
        StateHasChanged();
    }

    private void IncreaseCount(SaleInvoiceDetailModel requestModel)
    {
        requestModel.Quantity += 1;
        lstSaleInvoice!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Quantity = requestModel.Quantity; ;
        lstSaleInvoice!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Amount = (requestModel.Price * requestModel.Quantity);
        StateHasChanged();
    }

    private void DecreaseCount(SaleInvoiceDetailModel requestModel, int quantity)
    {
        if (requestModel.Quantity > 0)
        {
            requestModel.Quantity -= quantity;
            lstSaleInvoice!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Quantity = requestModel.Quantity;
            lstSaleInvoice!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Amount = (requestModel.Price * requestModel.Quantity);
            StateHasChanged();
        }
    }

    private async void SearchIcon()
    {
        Console.WriteLine(Search);
        if (Search.Length > 0)
        {
            lstProduct = ResponseModel.Data.Product.Where(x => x.ProductName.ToLower().Contains(Search.ToLower())).ToList();
        };
    }

    private void Pay()
    {
        Console.WriteLine(JsonConvert.SerializeObject(lstSaleInvoice));
        saleInvoiceFormType = EnumSaleInvoiceFormType.Checkout;
    }

    private void Back()
    {
        saleInvoiceFormType = EnumSaleInvoiceFormType.SaleProduct;
    }

    private bool IsProductSelected(ProductModel product)
    {
        return lstSaleInvoice != null && lstSaleInvoice.Any(x => x.ProductCode == product.ProductCode && x.Quantity > 0);
    }

    private async void CheckoutPay()
    {
        // Validate SaleInvoiceDetails
        if (lstSaleInvoice == null || lstSaleInvoice.Count == 0 || lstSaleInvoice.All(x => x.Quantity <= 0))
        {
            InjectService.ShowMessage("Please add at least one item to checkout.", EnumResponseType.Error);
            return;
        }

        // Validate TotalAmount
        var totalAmount = lstSaleInvoice.Sum(x => x.Amount);
        if (totalAmount <= 0)
        {
            InjectService.ShowMessage("Total amount must be greater than zero.", EnumResponseType.Error);
            return;
        }

        // Validate CustomerAccountNo (required by backend)
        if (string.IsNullOrWhiteSpace(reqModel.CustomerAccountNo))
        {
            InjectService.ShowMessage("Customer Account No is required.", EnumResponseType.Error);
            return;
        }

        // Validate PaymentAmount
        if (reqModel.PaymentAmount == null || reqModel.PaymentAmount <= 0)
        {
            InjectService.ShowMessage("Payment Amount must be greater than zero.", EnumResponseType.Error);
            return;
        }

        // Validate ReceiveAmount
        if (reqModel.ReceiveAmount == null || reqModel.ReceiveAmount <= 0)
        {
            InjectService.ShowMessage("Receive Amount must be greater than zero.", EnumResponseType.Error);
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
            reqModel.CustomerCode = "C_001"; // Default customer code
        }

        // Set model properties
        reqModel.SaleInvoiceDetails = lstSaleInvoice;
        reqModel.SaleInvoiceDateTime = DateTime.Now;
        reqModel.TotalAmount = totalAmount;
        reqModel.StaffCode = "S_001";
        reqModel.PaymentType = "KBZPay";
        reqModel.Discount = reqModel.Discount < 0 ? 0 : reqModel.Discount;
        reqModel.Tax = reqModel.Tax < 0 ? 0 : reqModel.Tax;
        //reqModel.Change = change;

        Console.WriteLine(JsonConvert.SerializeObject(reqModel).ToString());
        var response = await HttpClientService.ExecuteAsync<SaleInvoiceResponseModel>(
            Endpoints.SaleInvoice,
            EnumHttpMethod.Post,
            reqModel
            );
        Console.WriteLine(JsonConvert.SerializeObject(response).ToString());
        if (response.IsError)
        {
            InjectService.ShowMessage(response.Message, EnumResponseType.Error);
            return;
        }

        InjectService.ShowMessage(response.Message, EnumResponseType.Success);
        saleInvoiceFormType = EnumSaleInvoiceFormType.SaleProduct;
        lstSaleInvoice = new List<SaleInvoiceDetailModel>();
        StateHasChanged();
    }
}

public enum EnumSaleInvoiceFormType
{
    SaleProduct,
    Checkout
}
