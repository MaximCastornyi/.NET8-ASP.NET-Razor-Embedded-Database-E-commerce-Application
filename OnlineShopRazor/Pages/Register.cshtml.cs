using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

        public RegisterModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public OnlineShopRazor.Models.Db.User User { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //====================================
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(User.Email);
            if (!match.Success)
            {
                ModelState.AddModelError("User.Email", "Email is not valid");
                return Page();
            }

            User.Email = User.Email.Trim();
            var prevUser = _context.Users.Any(x => x.Email == User.Email);
            if (prevUser == true)
            {
                ModelState.AddModelError("User.Email", "Email exists");
                return Page();
            }

            User.RegisterDate = DateTime.Now;
            User.IsAdmin = false;
            User.Password = User.Password.Trim();
            User.FullName = User.FullName.Trim();
            User.RecoveryCode = 0;
            //====================================
            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Login");
        }
    }
}
