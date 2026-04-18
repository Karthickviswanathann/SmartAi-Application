using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
namespace SkillTrackerFront.Services
{
    public class CustAuthStateProvider: AuthenticationStateProvider
    {
        private readonly IJSRuntime _Js;
        private readonly NavigationManager _Navigation;


        public CustAuthStateProvider(IJSRuntime Js, NavigationManager Navigation)
        {
            _Js = Js;
            _Navigation= Navigation;
        }



        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _Js.InvokeAsync<string>("sessionStorage.getItem", "authToken");

                //var token = await _protectedSessionStorage.GetItemAsync<string>("Sessiondata");
                ClaimsIdentity identity = new ClaimsIdentity();

                if (token != null || token != null)
                {

                    var handler = new JwtSecurityTokenHandler();
                    var expirytoken = handler.ReadJwtToken(token);

                    // The 'exp' claim is in Unix epoch time
                    var expirationTime = expirytoken.ValidTo;

                    // Check if the token has expired
                    // Adding a small buffer (e.g., 5 minutes) can be good practice to avoid clock skew issues
                    if (expirationTime < DateTime.UtcNow.AddMinutes(-1))
                    {
                        await _Js.InvokeVoidAsync("sessionStorage.removeItem", "authToken");
                    }
                    else
                    {
                        identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwtAuthType");
                    }

                }
                //var identity = token==null
                //    ? new ClaimsIdentity()
                //    : new ClaimsIdentity(ParseClaimsFromJwt(token.JWT), "jwtAuthType");

                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async void NotifyUserAuthentication(string token)
        {

            try
            {

                DataAcces dataAcces = new DataAcces();
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwt = tokenHandler.ReadJwtToken(token);
                string EncrptedClaim = jwt.Claims.FirstOrDefault(c => c.Type == "encrptedClaims")?.Value;
                var decryptedclaim = dataAcces.Decrypturls(EncrptedClaim);
                var decrptjson = JsonConvert.DeserializeObject<Dictionary<string, string>>(decryptedclaim);
                string UserID = decrptjson["UserId"];
                string Email = decrptjson["Email"];
                string Status = decrptjson["IsActive"];
               
                var _nbf = jwt.Claims.FirstOrDefault(claim => claim.Type == "nbf")?.Value;
                var _exp = jwt.Claims.FirstOrDefault(claim => claim.Type == "exp")?.Value;
                var _nameId = jwt.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                string UsName = decrptjson["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];

                await _Js.InvokeVoidAsync("sessionStorage.removeItem", "authToken");

                await _Js.InvokeVoidAsync("sessionStorage.setItem", "authToken", token);
                await _Js.InvokeVoidAsync("sessionStorage.setItem", "Username", UsName);
                //await _protectedSessionStorage.SetItemAsync("Sessiondata", Sesiondata.JWT);


                var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwtAuthType");
                var user = new ClaimsPrincipal(identity);
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

            }
            catch (Exception ex)
            {
                showmessage("Error", ex.Message, 30000);

            }


        }

        public async void NotifyUserLogout()
        {
            await _Js.InvokeVoidAsync("sessionStorage.removeItem", "authToken");
            await _Js.InvokeVoidAsync("sessionStorage.removeItem", "ThemeColor");
            await _Js.InvokeVoidAsync("sessionStorage.removeItem", "Elementcolor");
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
        }
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Claims;
        }


        public async void showmessage(string type, string message,int duration)
        {
            await _Js.InvokeVoidAsync("barmessage", type, message, duration);

        }

    }
}
