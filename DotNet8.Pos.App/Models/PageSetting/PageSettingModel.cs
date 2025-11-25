namespace DotNet8.Pos.App.Models.PageSetting;

public class PageSettingModel(int pageNo, int pageSize, int pageCount, int totalCount)
{
    public int TotalCount { get; set; } = totalCount;
    public int PageCount { get; set; } = pageCount;
    public int PageNo { get; set; } = pageNo;
    public int PageSize { get; set; } = pageSize;
    public bool IsEndOfPage => PageNo == PageCount;
}