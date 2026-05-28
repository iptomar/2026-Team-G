// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace _2026_Team_G.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                // REMOVIDA a verificação de confirmação de email
                if (user == null)
                {
                    // Não revelar que o utilizador não existe
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password - 2026 Team G",
                    $@"<html>
                        <body style='font-family: Arial, sans-serif;'>
                            <h2 style='color: #7cb13b;'>Recuperação de Palavra-passe</h2>
                            <p>Recebemos um pedido para redefinir a sua palavra-passe.</p>
                            <p>Clique no link abaixo para criar uma nova palavra-passe:</p>
                            <p><a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='background-color: #7cb13b; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Redefinir Palavra-passe</a></p>
                            <p>Se não foi você que pediu, ignore este email.</p>
                            <p>O link é válido por 24 horas.</p>
                            <br>
                            <p>Equipa 2026 Team G</p>
                        </body>
                    </html>");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}