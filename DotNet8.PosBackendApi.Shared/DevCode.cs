using Newtonsoft.Json;

namespace DotNet8.PosBackendApi.Shared;

public static class DevCode
{
    public static string ToHash(this string password, string sharedKey)
    {
        var data = Hash.Create(HashType.SHA256, password, sharedKey, false);
        return data;
    }

    public static DateTime ToDateTime(this DateTime? dateTime)
    {
        return Convert.ToDateTime(dateTime);
    }

    public static string GenerateCode(this string code, string prefix, int length = 5)
    {
        string generateCode = string.Empty;
        if (string.IsNullOrWhiteSpace(code))
        {
            string defaultCode = "1";
            generateCode = $"{prefix}{defaultCode.PadLeft(length, '0')}";
            goto result;
        }

        //ode = code.Substring(1);
        code = code.Replace(prefix, "");
        int convertToInt = Convert.ToInt32(code) + 1;
        generateCode = $"{prefix}{convertToInt.ToString().PadLeft(length, '0')}";
    result:
        return generateCode;
    }

    public static string GenerateProductCategoryCode(this string code, string prefix, int length = 5)
    {
        string generateCode = string.Empty;
        if (string.IsNullOrWhiteSpace(code))
        {
            string defaultCode = "1";
            generateCode = $"{prefix}{defaultCode.PadLeft(length, '0')}";
            goto result;
        }

        //code = code.Substring(1);
        code = code.Replace(prefix, "");
        int convertToInt = Convert.ToInt32(code) + 1;
        generateCode = $"{prefix}{convertToInt.ToString().PadLeft(length, '0')}";
    result:
        return generateCode;
    }

    public static string GenerateTownshipCode(this string code, string prefix, int length = 5)
    {
        string generateCode = string.Empty;
        if (string.IsNullOrWhiteSpace(code))
        {
            string defaultCode = "1";
            generateCode = $"{prefix}{defaultCode.PadLeft(length, '0')}";
            goto result;
        }

        //code = code.Substring(1);
        code = code.Replace(prefix, "");
        int convertToInt = Convert.ToInt32(code) + 1;
        generateCode = $"{prefix}{convertToInt.ToString().PadLeft(length, '0')}";
    result:
        return generateCode;
    }

    public static string GenerateStateCode(this string code, string prefix, int length = 5)
    {
        string generateCode = string.Empty;
        if (string.IsNullOrWhiteSpace(code))
        {
            string defaultCode = "1";
            generateCode = $"{prefix}{defaultCode.PadLeft(length, '0')}";
            goto result;
        }

        //code = code.Substring(1);
        code = code.Replace(prefix, "");
        int convertToInt = Convert.ToInt32(code) + 1;
        generateCode = $"{prefix}{convertToInt.ToString().PadLeft(length, '0')}";
    result:
        return generateCode;
    }

    public static IQueryable<TSource> Pagination<TSource>(this IQueryable<TSource> source, int pageNo, int pageSize)
    {
        return source
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize);
    }

    public static string ToName2HexColor(this string name)
    {
        // Convert name to Base64
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(name);
        string base64 = Convert.ToBase64String(bytes);

        // Take a substring of the Base64 string
        string substring = base64.Substring(0, 6);

        // Convert the substring to hex
        string hex = "";
        foreach (char c in substring)
        {
            hex += ((int)c).ToString("X2");
        }

        // Ensure hex has exactly 6 characters
        if (hex.Length < 6)
        {
            hex = hex.PadRight(6, '0');
        }
        else if (hex.Length > 6)
        {
            hex = hex.Substring(0, 6);
        }

        return "#" + hex;
    }

    public static string? ToJson<T>(this T? obj, bool format = false)
    {
        if (obj == null) return string.Empty;
        string? result;
        if (obj is string)
        {
            result = obj.ToString();
            goto Result;
        }

        var settings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddTHH:mm:ss.sssZ" };
        result = format
            ? JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, settings)
            : JsonConvert.SerializeObject(obj, settings);
    Result:
        return result;
    }
}