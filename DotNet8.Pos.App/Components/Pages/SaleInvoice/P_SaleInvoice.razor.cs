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
    }

    private void IncreaseCount(SaleInvoiceDetailModel requestModel)
    {
        requestModel.Quantity += 1;
        lstSaleInvoice!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Quantity = requestModel.Quantity; ;
        lstSaleInvoice!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Amount = (requestModel.Price * requestModel.Quantity);
    }

    private void DecreaseCount(SaleInvoiceDetailModel requestModel, int quantity)
    {
        if (requestModel.Quantity > 0)
        {
            requestModel.Quantity -= quantity;
            lstSaleInvoice!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Quantity = requestModel.Quantity;
            lstSaleInvoice!.Where(x => x.ProductCode == requestModel.ProductCode).FirstOrDefault()!.Amount = (requestModel.Price * requestModel.Quantity);
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

    private async void CheckoutPay()
    {
        reqModel.SaleInvoiceDetails = lstSaleInvoice;
        reqModel.SaleInvoiceDateTime = DateTime.Now;
        reqModel.TotalAmount = lstSaleInvoice.Sum(x => x.Amount);
        reqModel.StaffCode = "S_001";
        reqModel.PaymentType = "KBZPay";
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
