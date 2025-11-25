namespace DotNet8.Pos.App.Models.Customer;

public class CustomerRequestModel
{
    public int CustomerId { get; set; }

    public string CustomerCode { get; set; } = string.Empty;

    public string CustomerName { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public string TownshipCode { get; set; } = null!;
}