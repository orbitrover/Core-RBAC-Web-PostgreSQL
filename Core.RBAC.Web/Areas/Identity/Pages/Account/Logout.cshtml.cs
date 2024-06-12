// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Core.RBAC.Web.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUserExtended> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IDataAccessService _dataAccessService;
        public LogoutModel(SignInManager<IdentityUserExtended> signInManager, ILogger<LogoutModel> logger, IDataAccessService dataAccessService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _dataAccessService = dataAccessService;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            var online = false;
            await _dataAccessService.MakeUserStatusOnline(_signInManager, online, User.Identity.Name);
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
