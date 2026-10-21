using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Coupons;

[Authorize(Roles = "admin")]
public class EditModel : PageModel
{
    private readonly OnlineDataBaseContext _context;

    public EditModel(OnlineDataBaseContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Coupon Coupon { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var coupon = await _context.Coupons.FirstOrDefaultAsync(m => m.Id == id);
        if (coupon is null)
        {
            return NotFound();
        }
        Coupon = coupon;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Coupon).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CouponExists(Coupon.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool CouponExists(int id)
    {
        return _context.Coupons.Any(e => e.Id == id);
    }
}
