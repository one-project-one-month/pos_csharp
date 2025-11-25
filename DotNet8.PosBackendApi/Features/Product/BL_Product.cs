namespace DotNet8.PosBackendApi.Features.Product;

public class BL_Product
{
    private readonly DL_Product _dL_Product;

    public BL_Product(DL_Product dL_Product) => _dL_Product = dL_Product;

    public async Task<ProductListResponseModel> GetProduct()
    {
        var response = await _dL_Product.GetProduct();
        return response;
    }
    public async Task<ProductListResponseModel> GetProduct(int pageNo, int pageSize, string? search = null)
    {
        var response = await _dL_Product.GetProduct(pageNo, pageSize, search);
        return response;
    }

    public async Task<ProductResponseModel> GetProductByCode(string productCode)
    {
        if (productCode is null) throw new Exception("productCode is null");
        var response = await _dL_Product.GetProductByCode(productCode);
        return response;
    }

    public async Task<MessageResponseModel> Create(ProductModel requestModel)
    {
        CheckProductNullValue(requestModel);
        var response = await _dL_Product.Create(requestModel);
        return response;
    }

    public async Task<MessageResponseModel> Update(int id, ProductModel requestModel)
    {
        if (id <= 0) throw new Exception("productCode is null");
        //CheckProductNullValue(requestModel);
        var response = await _dL_Product.Update(id, requestModel);
        return response;
    }

    public async Task<MessageResponseModel> Delete(int id)
    {
        if (id <= 0) throw new Exception("productCode is null");
        var response = await _dL_Product.Delete(id);
        return response;
    }

    private static void CheckProductNullValue(ProductModel product)
    {
        if (product == null)
            throw new Exception("product is null.");

        if (string.IsNullOrWhiteSpace(product.ProductName))
            throw new Exception("product.ProductName is null.");

        /*if (string.IsNullOrWhiteSpace(product.ProductCode))
        {
            throw new Exception("product.ProductCode is null.");
        }*/

        if (string.IsNullOrWhiteSpace(product.ProductCategoryCode))
            throw new Exception("product.ProductCategoryCode is null.");

        if (product.Price <= 0)
            throw new Exception("product.Price must be greater than zero.");
    }
}