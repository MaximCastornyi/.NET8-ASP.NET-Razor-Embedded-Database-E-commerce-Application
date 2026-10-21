using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Orders;

[Authorize(Roles = "admin")]
public class DeleteModel : PageModel
{
    private readonly OnlineDataBaseContext _context;

    public DeleteModel(OnlineDataBaseContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Order Order { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var order = await _context.Orders.FirstOrDefaultAsync(m => m.Id == id);
        if (order is null)
        {
            return NotFound();
        }
        else
        {
            Order = order;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            Order = order;
            _context.Orders.Remove(Order);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
