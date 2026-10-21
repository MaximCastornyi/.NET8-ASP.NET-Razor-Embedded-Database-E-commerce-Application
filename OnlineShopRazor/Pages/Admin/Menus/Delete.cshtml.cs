using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Menus;


[Authorize(Roles = "admin")]
public class DeleteModel : PageModel
{
    private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

    public DeleteModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Order Order { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Orders.FirstOrDefaultAsync(m => m.Id == id);

        if (order == null)
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
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            Order = order;
            _context.Orders.Remove(Order);
            //-------- delete order details--------
            var orderDetails = await _context.OrderDetails.Where(x => x.OrderId == id).ToListAsync();
            _context.OrderDetails.RemoveRange(orderDetails);
            //-------------------------------------
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}