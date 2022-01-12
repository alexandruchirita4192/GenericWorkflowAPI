// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Claims;
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

        /// <summary>
        /// Get claims (duplicate claims become an array)
        /// </summary>
        public static IEnumerable<Claim> GetClaims(IEnumerable<ClaimsIdentity> identities, IEnumerable<Claim> claims)
        {
            var claimsList = new List<Claim>();

            foreach (var identity in identities)
            {
                var identityClaims = identity.Claims;
                AddClaims(claimsList, identityClaims);
            }

            AddClaims(claimsList, claims);
            
            return claimsList;
        }

        private static void AddClaims(List<Claim> claimsList, IEnumerable<Claim> claims)
        {
            if (claims == null || claimsList == null)
                return;

            foreach (var claim in claims)
            {
                if (!IsClaimRequired(claim))
                    continue;
                
                var newClaim = TransformClaim(claim);

                claimsList.Add(newClaim ?? claim);
            }
        }
        
        private static Claim TransformClaim(Claim claim)
        {
            if (claim == null)
                return claim;

            // "preferred_usernamed" contained the username, so it's sent as ClaimTypes.NameIdentifier
            if (claim.Type == "preferred_username" && claim.ValueType == ClaimValueTypes.String)
                return new Claim(ClaimTypes.Name, claim.Value, claim.ValueType);

            // Use some default naming for claim types to find them by the same constants
            if (claim.Type == "given_name" && claim.ValueType == ClaimValueTypes.String)
                return new Claim(ClaimTypes.GivenName, claim.Value, claim.ValueType);
            if (claim.Type == "family_name" && claim.ValueType == ClaimValueTypes.String)
                return new Claim(ClaimTypes.Surname, claim.Value, claim.ValueType);

            return claim;
        }

        private static bool IsClaimRequired(Claim claim)
        {
            if (claim == null)
                return false;

            //if (claim.Type == "email_verified" && claim.ValueType == ClaimValueTypes.String)
            //    return false;
            //if (claim.Type == "AspNet.Identity.SecurityStamp" && claim.ValueType == ClaimValueTypes.String)
            //    return false;

            return true;
        }
    }
}