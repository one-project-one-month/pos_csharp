namespace DotNet8.Pos.App.Models.Customer;

public class CustomerParamsModel(int customerID, 
    string customerName, 
    string mobileNo, 
    DateTime? dateOfBirth,
    string gender, 
    string statusCode, 
    string townshipCode)
{
    public int CustomerId { get; set; } = customerID;

    public string CustomerName { get; set; } = customerName;

    public string MobileNo { get; set; } = mobileNo;

    public DateTime? DateOfBirth { get; set; } = dateOfBirth;

    public string Gender { get; set; } = gender;

    public string StateCode { get; set; } = statusCode;

    public string TownshipCode { get; set; } = townshipCode;
}

public class CustomerParamsModelV1
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } 

    public string MobileNo { get; set; }

    public DateTime? DateOfBirth { get; set; } 

    public string Gender { get; set; } 

    public string StateCode { get; set; }

    public string TownshipCode { get; set; } 
}