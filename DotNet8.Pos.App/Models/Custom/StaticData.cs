namespace DotNet8.Pos.App.Models.Custom;

public class StaticData
{
    public static List<KeyValueModel> Gender()
    {
        return new List<KeyValueModel>()
        {
            new("0", "--Select One--"),
            new("Male", "Male"),
            new("Female", "Female"),
        };
    }

    public static List<KeyValueModel> TaxType()
    {
        return new List<KeyValueModel>()
        {
            new("Percentage", "Percentage"),
            new("Fixed", "Fixed"),
        };
    }
}