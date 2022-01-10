// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using GenericWorkflowAPI.Core.Attributes;
using GenericWorkflowAPI.IdentityServer4;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerHost.Quickstart.UI
{
    [SecurityHeaders]
    [Authorize]
    public class DiagnosticsController : Controller
    {
        private readonly IdentityServerTools _tools;

        public DiagnosticsController(IdentityServerTools tools)
        {
            _tools = tools;
        }

        public async Task<IActionResult> Index()
        {
            var localAddresses = new string[] { "127.0.0.1", "::1", HttpContext.Connection.LocalIpAddress.ToString() };
            if (!localAddresses.Contains(HttpContext.Connection.RemoteIpAddress.ToString()))
            {
                return NotFound();
            }

            var model = new DiagnosticsViewModel(await HttpContext.AuthenticateAsync());
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [AjaxOnly]
        public async Task<IActionResult> GetToken()
        {
            var token = await _tools.IssueClientJwtAsync(
                clientId: "client_id",
                lifetime: 3600,
                scopes: Config.ApiScopes.Select(s => s.Name),
                audiences: Config.ApiAudiences);

            return Ok(token);
        }
    }
}