namespace DotNet8.Pos.App.Services;

public static class Endpoints
{
    public static string ProductCategory { get; } = "api/v1/product-categories";
    public static string Customer { get; } = "api/v1/customers";
    public static string Product { get; } = "api/v1/products";
    public static string Staff { get; } = "api/v1/staffs";
    public static string Township { get; } = "api/v1/townships";
    public static string State { get; } = "api/v1/states";
    public static string SaleInvoice { get; } = "api/v1/sale-invoices";
    public static string Tax { get; } = "api/v1/taxes";
    public static string Report { get; } = "api/v1/report";
    public static string Shop { get; } = "api/v1/shops";
    public static string Dashboard { get; } = "api/v1/dashboard";
    public static string Login { get; } = "api/v1/auth/login";
    public static string WithPagination(this string url, int pageNo, int pageSize)
    {
        return $"{url}/{pageNo}/{pageSize}";
    }
}