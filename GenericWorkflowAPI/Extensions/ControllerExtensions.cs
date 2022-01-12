using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GenericWorkflowAPI.Helpers
{
    public static class ControllerExtensions
    {
        public static Domain.IdentityUser GetIdentityUser(this ControllerBase controller)
        {
            var userId = controller.GetUserId();
            if (userId == null)
                return null;

            string? userName = controller.GetUserName();
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            var firstUserEmail = controller.GetFirstClaimValue(ClaimTypes.Email);
            var identityClaims = controller.GetIdentityUserClaimList(userId.Value);

            var identityUser = new Domain.IdentityUser(userName)
            {
                Id = userId.Value,
                Email = firstUserEmail,
                Claims = identityClaims
            };

            return identityUser;
        }

        public static List<IdentityUserClaim<string>>? GetIdentityUserClaimList(this ControllerBase controller, long userId)
        {
            return controller.HttpContext?.User?.Claims?
                            .Select(c => new IdentityUserClaim<string>
                            {
                                ClaimType = c.Type,
                                ClaimValue = c.Value,
                                UserId = userId.ToString()
                            })?.ToList();
        }

        public static string? GetFirstClaimValue(this ControllerBase controller, string claimType)
        {
            var claimValue = controller.HttpContext
                ?.User
                ?.Claims
                ?.FirstOrDefault(c => c.Type == claimType)
                ?.Value;

            return claimValue;
        }

        public static long? GetUserId(this ControllerBase controller)
        {
            var userIdAsString = controller.GetFirstClaimValue(ClaimTypes.NameIdentifier);
            var userId = 0l;

            if (string.IsNullOrWhiteSpace(userIdAsString) || !long.TryParse(userIdAsString, out userId))
                return null;

            return userId;
        }

        private static string? GetUserName(this ControllerBase controller)
        {
            return controller.HttpContext?.User?.Identity?.Name;
        }

        public static string? GetUserGivenName(this ControllerBase controller)
        {
            return controller.GetFirstClaimValue(ClaimTypes.GivenName);
        }

        public static string? GetUserSurname(this ControllerBase controller)
        {
            return controller.GetFirstClaimValue(ClaimTypes.Surname);
        }

        public static List<string> GetUserRoles(this ControllerBase controller)
        {
            var userRoles = new List<string>();

            var roles = controller.HttpContext
                ?.User
                ?.Claims
                ?.Where(c => c.Type == ClaimTypes.Role && c.ValueType == ClaimValueTypes.String);

            if (roles != null && roles.Any())
                userRoles.AddRange(roles.Select(c => c.Value));

            return userRoles;
        }
    }
}