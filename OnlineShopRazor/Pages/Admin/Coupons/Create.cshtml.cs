using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Coupons;

[Authorize(Roles = "admin")]
public class CreateModel : PageModel
{
    private readonly OnlineDataBaseContext _context;

    public CreateModel(OnlineDataBaseContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Coupon Coupon { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Coupons.Add(Coupon);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
