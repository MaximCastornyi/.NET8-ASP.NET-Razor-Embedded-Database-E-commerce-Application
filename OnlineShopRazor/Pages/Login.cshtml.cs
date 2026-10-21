using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace OnlineShopRazor.Pages
{
    public class LoginModel : PageModel
    {
        private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

        public LoginModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
        {
            _context = context;

        }

        [BindProperty, Required]
        public string? UserName { get; set; }

        [BindProperty, Required]
        public string? Password { get; set; }

        public int MyProperty { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //---------------
            var findUser = _context.Users.FirstOrDefault(x => x.Email == UserName.Trim() && x.Password == Password.Trim());
            if (findUser == null)
            {
                ModelState.AddModelError("UserName", "User name or password is not valid!");
                return Page();
            }
            //----------------
            // Create claims for the authenticated user
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, findUser.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, findUser.FullName));
            claims.Add(new Claim(ClaimTypes.Email, findUser.Email));
            claims.Add(new Claim(ClaimTypes.Role, findUser.IsAdmin ? "admin" : "user"));

            // Create an identity based on the claims
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Create a principal based on the identity
            var principal = new ClaimsPrincipal(identity);

            // Sign in the user with the created principal
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Redirect("/");

        }
    }
}
