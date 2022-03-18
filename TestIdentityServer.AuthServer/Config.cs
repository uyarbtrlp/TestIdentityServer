using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestIdentityServer.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("resource_api1"){ 
                
                    Scopes = {

                        "api1.read","api1.write", "api1.update"
                    },
                    ApiSecrets = new [] { new Secret("secretapi1".Sha256())}
                },
                new ApiResource("resource_api2")
                {
                    Scopes = {

                        "api2.read","api2.write", "api2.update"
                    },
                    ApiSecrets = new [] { new Secret("secretapi2".Sha256())}
                }
            };
        }


        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>()
            {
                new ApiScope("api1.read","Read privilege for API 1"),
                new ApiScope("api1.write","Write privilege for API 1"),
                new ApiScope("api1.update","Update privilege for API 1"),
                new ApiScope("api2.read","Read privilege for API 2"),
                new ApiScope("api2.write","Write privilege for API 2"),
                new ApiScope("api2.update","Update privilege for API 2"),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>(){
                new Client()
                {
                    ClientId = "Client1",
                    ClientName = "Client 1 Application",
                    ClientSecrets = new []
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"api1.read"}
                },
                new Client()
                {
                    ClientId = "Client2",
                    ClientName = "Client 2 Application",
                    ClientSecrets = new []
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"api1.read","api2.write","api2.update","api1.update"}
                },
                new Client()
                {
                    ClientId = "Client1-Mvc",
                    ClientName = "Client1-Mvc Application",
                    ClientSecrets = new []
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequirePkce = false,
                    RedirectUris = new List<string>{"https://localhost:5006/signin-oidc"},
                    PostLogoutRedirectUris = new List<string>{"https://localhost:5006/signout-callback-oidc" },
                    AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, "api1.read", IdentityServerConstants.StandardScopes.OfflineAccess},
                    AccessTokenLifetime = 2*60*60,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime =  (int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),

            };
        }

        public static IEnumerable<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser
                { SubjectId = "1", Username = "baturalp", Password = "password", Claims = new List<Claim>()
                    {
                        new Claim("given_name","Baturalp"),
                        new Claim("family_name","Uyar")
                    }
                },
                new TestUser
                { SubjectId = "2", Username = "begum", Password = "password", Claims = new List<Claim>()
                    {
                        new Claim("given_name","Begüm"),
                        new Claim("family_name","Uyar")
                    }
                }
            };
        }
    }
}
