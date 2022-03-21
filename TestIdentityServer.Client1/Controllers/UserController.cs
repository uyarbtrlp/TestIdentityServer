using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestIdentityServer.Client1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task LogOut()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        public async Task<IActionResult> GetRefreshToken()
        {
            HttpClient client = new HttpClient();
            var discovery = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (discovery.IsError)
            {
                //log
            }
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            var token = await client.RequestRefreshTokenAsync(new RefreshTokenRequest() {
            
                ClientId = "Client1-Mvc",
                ClientSecret = "secret",
                RefreshToken = refreshToken,
                Address = discovery.TokenEndpoint
            
            });

            if (token.IsError)
            {
                //log
            }

            var tokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken(){Name =OpenIdConnectParameterNames.IdToken, Value = token.IdentityToken},
                new AuthenticationToken(){Name =OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken},
                new AuthenticationToken(){Name =OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken},
                new AuthenticationToken(){Name =OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)},
            };

            var authenticationResult = await HttpContext.AuthenticateAsync();

            var properties = authenticationResult.Properties;
            properties.StoreTokens(tokens);

            await HttpContext.SignInAsync("Cookies", authenticationResult.Principal, properties);





            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public IActionResult AdminAction()
        {
            return View();
        }
        [Authorize(Roles = "admin,customer")]
        public IActionResult CustomerAction()
        {
            return View();
        }
    }
}
