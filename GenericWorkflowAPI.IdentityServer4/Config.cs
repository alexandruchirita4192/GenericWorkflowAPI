// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using IdentityServer4.Models;

namespace GenericWorkflowAPI.IdentityServer4
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("GenericWorkflowAPI"),
            };

        public static IEnumerable<string> ApiAudiences =>
            new string[]
            {
                "GenericWorkflowAPI"
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // Machine 2 machine client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("5ea13cbf-604e-447f-8645-0836c1b1d8e2".Sha256()) },

                    AllowedScopes = { "GenericWorkflowAPI" }
                },
            };
    }
}