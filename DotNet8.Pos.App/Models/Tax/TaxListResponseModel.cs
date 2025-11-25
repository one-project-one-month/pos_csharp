using DotNet8.PosBackendApi.Models;

namespace DotNet8.Pos.App.Models.Tax;

public class TaxListResponseModel
{
    public TaxDataModel Data { get; set; }
    public MessageResponseModel MessageResponse { get; set; }
    public PageSettingModel PageSetting { get; set; }
}

public class TaxDataModel
{
    public List<TaxModel> Tax { get; set; }
}
