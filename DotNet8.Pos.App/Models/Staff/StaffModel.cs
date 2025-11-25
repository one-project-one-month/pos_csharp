namespace DotNet8.Pos.App.Models.Staff;

public class StaffModel
{
    public int StaffId { get; set; }
    public string StaffCode { get; set; } = null!;
    public string StaffName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string MobileNo { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public string Position { get; set; } = null!;
    public string Password { get; set; } = null!;
}