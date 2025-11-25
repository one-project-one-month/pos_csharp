namespace DotNet8.Pos.App.Models.Staff;

public class StaffListResponseModel : ResponseModel
{
    public StaffDataModel Data { get; set; }
    public PageSettingModel PageSetting { get; set; }
}