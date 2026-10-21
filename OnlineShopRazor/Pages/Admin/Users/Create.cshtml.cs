using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Users;

[Authorize(Roles = "admin")]
public class CreateModel : PageModel
{
    private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

    public CreateModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
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

        _context.Users.Add(User);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

