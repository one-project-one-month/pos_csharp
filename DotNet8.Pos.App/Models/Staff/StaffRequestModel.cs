namespace DotNet8.Pos.App.Models.Staff;

public class StaffRequestModel
{
    public int StaffId { get; set; }
    public string? StaffCode { get; set; } 
    public string StaffName { get; set; } 
    public DateTime? DateOfBirth { get; set; }
    public string MobileNo { get; set; } 
    public string Address { get; set; } 
    public string Gender { get; set; }
    public string Position { get; set; }
    public string Password { get; set; }
}