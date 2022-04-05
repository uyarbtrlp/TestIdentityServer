using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using TestIdentityServer.Client1.Models;

namespace TestIdentityServer.Client1.Controllers
{
    public class LoginController : Controller
    {
        IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            var client = new HttpClient();
            var discovery = await client.GetDiscoveryDocumentAsync(_configuration["AuthServerUrl"]);
            if (discovery.IsError)
            {

            }
            var password = new PasswordTokenRequest()
            {
                Address = discovery.TokenEndpoint,
                UserName = loginViewModel.Email,
                Password = loginViewModel.Password,
                ClientId = _configuration["ClientResourceOwner:ClientId"],
                ClientSecret = _configuration["ClientResourceOwner:ClientSecret"]
            };
            var token = await client.RequestPasswordTokenAsync(password);

            if (token.IsError)
            {
                ModelState.AddModelError("","Wrong email or password.");

                return View();
            }

            var userInfoRequest = new UserInfoRequest()
            {
                Token = token.AccessToken,
                Address = discovery.UserInfoEndpoint
            };

            var userInfo = await client.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError)
            {

            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, "Cookies","name","role");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken(){Name =OpenIdConnectParameterNames.IdToken, Value = token.IdentityToken},
                new AuthenticationToken(){Name =OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken},
                new AuthenticationToken(){Name =OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken},
                new AuthenticationToken(){Name =OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)},
            });

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,authenticationProperties);
            return RedirectToAction("Index", "User");
        }
    }
}
