using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;
using TaskManager.Class;
using TaskManager.Database;
using TaskManager.Sha512;

namespace TaskManager.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly TakClassDatabase _context;

        public SignUpModel(TakClassDatabase context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password1 { get; set; }
        [BindProperty]
        public string Password2 { get; set; }

        public bool ShowError { get; private set; }
        public string ErrorMessage { get; private set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Password1 != Password2)
            {
                ShowError = true;
                ErrorMessage = "Пароли не совпадают.";
                return Page();
            }

            var user = new User
            {
                Username = Username,
                Password = HashHelper.ComputeSha512Hash(Password1)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Username),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            ShowError = false;

            return RedirectToPage("/Index");
        }
    }
}
