namespace DotNet8.Pos.App.Models.Customer;

public class CustomerModel
{
    public int CustomerId { get; set; }

    public string CustomerCode { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public string TownshipCode { get; set; } = null!;
}