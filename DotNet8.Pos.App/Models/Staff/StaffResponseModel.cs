namespace DotNet8.Pos.App.Models.Staff;

public class StaffResponseModel : ResponseModel
{
    public DataModel Data { get; set; } = new DataModel();
}

public class DataModel
{
    public StaffModel Staff { get; set; }
}