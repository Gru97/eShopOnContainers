// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Identity.API
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
              
            };
        }
        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("orders", "Orders API"),
                new ApiResource("basket", "Basket API"),
                new ApiResource("catalog", "Product API"),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "api1" }
                },

                // MVC client using hybrid flow
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    RedirectUris = { "http://localhost:5001/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:5001/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api1" }
                },

                // SPA client using code flow + pkce
                new Client
                {
                    ClientId = "angular",
                    ClientName = "SPA Client",
                    ClientUri = "http://identityserver.io",
                    AlwaysIncludeUserClaimsInIdToken=true,  //if you want claims to be included in identitytoken
                    AlwaysSendClientClaims=true,        //if you want claims to be included in access token
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequirePkce = false,
                    IncludeJwtId=true,
                    RequireConsent=false,
                    
                  
                    RequireClientSecret = false,

                    RedirectUris =
                    {
                        "http://localhost:4200/error.html",
                        "http://localhost:4200/error",
                        "http://localhost:4200/basket",
                         "http://localhost:4200/catalog",

                        "http://localhost:4200/",
                        "http://localhost:4200/index.html",
                        
                    },
                    
                    PostLogoutRedirectUris = { "http://localhost:4200/index.html" },
                    AllowedCorsOrigins = { "http://localhost:4200" },

                    AllowedScopes = { "openid", "profile", "email", "address", "phone" , "orders" , "basket", "catalog"},
                            AllowAccessTokensViaBrowser = true
                }
            };
        }
    }
}