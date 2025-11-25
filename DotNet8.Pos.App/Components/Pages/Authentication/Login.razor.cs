using DotNet8.Pos.App.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Security.Claims;

namespace DotNet8.Pos.App.Components.Pages.Authentication;

public partial class Login
{
    private LoginRequestModel _loginRequest = new LoginRequestModel();

    [Inject]
    private CustomAuthStateProvider _authStateProvider { get; set; } = default!;

    public async Task LoginUser()
    {
        try
        {
            await InjectService.EnableLoading();
            var responseModel = await HttpClientService.ExecuteAsync<LoginResponseModel>(
                $"{Endpoints.Login}",
                EnumHttpMethod.Post,
                _loginRequest);

            if (responseModel != null && responseModel.Message != null)
            {
                if (responseModel.Message.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(responseModel.token))
                    {
                        await _authStateProvider.SetTokenAsync(responseModel.token);
                        InjectService.ShowMessage("Login Successful", EnumResponseType.Success);
                        NavigationManager.NavigateTo("/");
                    }
                    else
                    {
                        InjectService.ShowMessage("Login failed: Token is missing.", EnumResponseType.Error);
                    }
                }
                else
                {
                    InjectService.ShowMessage(responseModel.Message.Message ?? "Login Failed.", EnumResponseType.Error);
                }
            }
            else
            {
                InjectService.ShowMessage("Server response is invalid.", EnumResponseType.Error);
            }
        }
        catch (Exception ex)
        {
            InjectService.ShowMessage(ex.Message, EnumResponseType.Error);
        }
        finally
        {
            await InjectService.DisableLoading();
        }
    }
}
