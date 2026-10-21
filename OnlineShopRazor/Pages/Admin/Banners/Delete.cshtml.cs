using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Banners;

[Authorize(Roles = "admin")]
public class DeleteModel : PageModel
{
    private readonly OnlineDataBaseContext _context;

    public DeleteModel(OnlineDataBaseContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Banner Banner { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var banner = await _context.Banners.FirstOrDefaultAsync(m => m.Id == id);
        if (banner is null)
        {
            return NotFound();
        }
        else
        {
            Banner = banner;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var banner = await _context.Banners.FindAsync(id);
        if (banner != null)
        {
            Banner = banner;
            //-------------delete image---------------
            //-----------------
            string org_fn;
            org_fn = Directory.GetCurrentDirectory() + "\\wwwroot\\images\\banners\\" + Banner.ImageName;

            if (System.IO.File.Exists(org_fn))
            {
                System.IO.File.Delete(org_fn);
            }
            //-----------------
            //----------------------------------------
            _context.Banners.Remove(Banner);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
