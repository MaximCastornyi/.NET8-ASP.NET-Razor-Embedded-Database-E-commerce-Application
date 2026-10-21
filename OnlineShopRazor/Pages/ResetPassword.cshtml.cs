using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using OnlineShopRazor.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace OnlineShopRazor.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

        public ResetPasswordModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
        {
            _context = context;
        }



        [BindProperty, Required]
        public string? Email { get; set; }

        [BindProperty, Required]
        public string? NewPassword { get; set; }

        [BindProperty, Required]
        public int? RecoveryCode { get; set; }

        public void OnGet(string email)
        {
            Email = email;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ////-------------------------------------------

            var foundUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == Email && x.RecoveryCode == RecoveryCode);
            if (foundUser == null)
            {
                ModelState.AddModelError("Email", "Email or recovery password is not valid!");
                return Page();
            }

            ////-------------------------------------------

            foundUser.Password = NewPassword;

            _context.Users.Update(foundUser);
            _context.SaveChanges();

            ////-------------------------------------------

            return RedirectToPage("Login");
        }

    }
}
