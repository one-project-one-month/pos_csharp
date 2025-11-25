using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace DotNet8.Pos.App.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _js;
        private ClaimsPrincipal _user = new(new ClaimsIdentity());
        private const string Token = "accessToken";
        public CustomAuthStateProvider(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SetTokenAsync(string token)
        {
            if(!string.IsNullOrWhiteSpace(token))
            {
                await _js.InvokeVoidAsync("localStorage.setItem", Token, token);
                _user = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
        }

        public async Task ClearTokenAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", Token);
            _user = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", Token);
            ClaimsIdentity identity;
            if (!string.IsNullOrWhiteSpace(token))
            {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var _user = new ClaimsPrincipal(identity);

            return new AuthenticationState(_user);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Value is JsonElement e && e.ValueKind == JsonValueKind.Array)
                {
                    foreach (var v in e.EnumerateArray())
                        claims.Add(new Claim(kvp.Key, v.ToString()));
                }
                else
                    claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
            }
            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
