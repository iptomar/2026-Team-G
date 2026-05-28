// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;

namespace _2026_Team_G.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public RegisterConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public string Email { get; set; }
        public bool DisplayConfirmAccountLink { get; set; }
        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;
            returnUrl = returnUrl ?? Url.Content("~/");

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);

            // enviar email estilizado
            await _emailSender.SendEmailAsync(
                email,
                "Confirme o seu email - 2026 Team G",
                $@"<html>
                    <body style='font-family: Arial, sans-serif;'>
                        <h2 style='color: #7cb13b;'>Bem-vindo à 2026 Team G!</h2>
                        <p>Por favor, confirme a sua conta clicando no botão abaixo:</p>
                        <p><a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='background-color: #7cb13b; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>Confirmar Email</a></p>
                        <p>Se não foi você que criou esta conta, ignore este email.</p>
                        <br>
                        <p>Equipa 2026 Team G</p>
                    </body>
                </html>");

            // Opcional: mostrar link na página também (apenas para desenvolvimento)
            DisplayConfirmAccountLink = true;
            EmailConfirmationUrl = callbackUrl;

            return Page();
        }
    }
}