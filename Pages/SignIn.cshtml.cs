using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TaskManager.Database;
using TaskManager.Sha512;

namespace TaskManager.Pages
{
    public class SignInModel : PageModel
    {
        private readonly TakClassDatabase _context;

        public SignInModel(TakClassDatabase context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public bool ShowError { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == Username);

            if (user == null)
            {
                ShowError = true;
                return Page();
            }

            if (Password == HashHelper.ComputeSha512Hash(user.Password))
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, Username)
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToPage("/Index");
            }
            ShowError = true;
            return Page();
        }


    }

}
