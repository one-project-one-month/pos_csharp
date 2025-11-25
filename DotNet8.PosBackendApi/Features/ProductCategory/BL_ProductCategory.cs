namespace DotNet8.PosBackendApi.Features.ProductCategory;

public class BL_ProductCategory
{
    private readonly DL_ProductCategory _productCategory;

    public BL_ProductCategory(DL_ProductCategory productCategory) => _productCategory = productCategory;

    public async Task<ProductCategoryListResponseModel> GetProductCategory()
    {
        var response = await _productCategory.GetProductCategory();
        return response;
    }

    public async Task<ProductCategoryListResponseModel> GetProductCategory(int pageNo, int pageSize, string? search = null)
    {
        return await _productCategory.GetProductCategory(pageNo, pageSize, search);
    }

    public async Task<ProductCategoryResponseModel> GetProductCategoryByCode(string productCategoryCode)
    {
        if (productCategoryCode is null) throw new Exception("productCategoryCode is null");
        var response = await _productCategory.GetProductCategoryByCode(productCategoryCode);
        return response;
    }

    public async Task<MessageResponseModel> CreateProductCategory(ProductCategoryModel requestModel)
    {
        CheckProductNullValue(requestModel);
        var response = await _productCategory.CreateProductCategory(requestModel);
        return response;
    }

    public async Task<MessageResponseModel> UpdateProductCategory(int id, ProductCategoryModel requestModel)
    {
        if (id <= 0) throw new Exception("productCategoryCode is null");
        //CheckProductNullValue(requestModel);
        var response = await _productCategory.UpdateProductCategory(id, requestModel);
        return response;
    }

    public async Task<MessageResponseModel> DeleteProductCategory(int id)
    {
        if (id <= 0) throw new Exception("productCategoryId is null");
        var response = await _productCategory.DeleteProductCategory(id);
        return response;
    }

    private void CheckProductNullValue(ProductCategoryModel productCategory)
    {
        if (productCategory == null)
            throw new Exception("productCategory is null.");

        if (string.IsNullOrWhiteSpace(productCategory.ProductCategoryName))
            throw new Exception("ProductCategoryName is null.");

        if (string.IsNullOrEmpty(productCategory.ProductCategoryCode))
            throw new Exception("ProductCateoryCode is null.");

        //if (string.IsNullOrWhiteSpace(productCategory.ProductCategoryCode))
        //{
        //    throw new Exception("ProductCategoryCode is null.");
        //}
    }
}