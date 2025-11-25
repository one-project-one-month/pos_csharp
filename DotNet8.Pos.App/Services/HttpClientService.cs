using System.Text;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace DotNet8.Pos.App.Services;

public class HttpClientService
{
    private readonly HttpClient _httpClient;

    public HttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T> ExecuteAsync<T>(string endpoint, EnumHttpMethod httpMethod, object? requestModel = null)
    {
        HttpResponseMessage? response = null;
        HttpContent content = null;
        if (requestModel is not null)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            content = new StringContent(json, Encoding.UTF8, Application.Json);
        }
        switch (httpMethod)
        {
            case EnumHttpMethod.Get:
                response = await _httpClient.GetAsync(endpoint);
                break;
            case EnumHttpMethod.Post:
                response = await _httpClient.PostAsync(endpoint, content);
                break;
            case EnumHttpMethod.Patch:
                response = await _httpClient.PatchAsync(endpoint, content);
                break;
            case EnumHttpMethod.Delete:
                response = await _httpClient.DeleteAsync(endpoint);
                break;
            case EnumHttpMethod.None:
            default:
                throw new Exception("Invalid EnumHttpMethod.");
        }
        //if (response.IsSuccessStatusCode)
        //{
           
        //}
        var responseJson = await response.Content.ReadAsStringAsync();
        var model = JsonConvert.DeserializeObject<T>(responseJson);
        return model!;
    }
}

public enum EnumHttpMethod
{
    None,
    Get,
    Post,
    Patch,
    Delete
}