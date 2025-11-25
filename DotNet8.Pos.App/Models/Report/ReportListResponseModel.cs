namespace DotNet8.Pos.App.Models.Report;

public class ReportListResponseModel : ResponseModel
{
    public ReportDataModel Data {  get; set; }
    public PageSettingModel PageSetting { get; set; }
}