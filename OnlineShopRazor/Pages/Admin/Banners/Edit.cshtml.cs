using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Banners;

[Authorize(Roles = "admin")]
public class EditModel : PageModel
{
    private readonly OnlineDataBaseContext _context;

    public EditModel(OnlineDataBaseContext context)
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
        Banner = banner;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync(IFormFile? ImageFile)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        //------------update image--------------------
        if (ImageFile != null)
        {
            //-----------------
            string org_fn;
            org_fn = Directory.GetCurrentDirectory() + "\\wwwroot\\images\\banners\\" + Banner.ImageName;

            if (System.IO.File.Exists(org_fn))
            {
                System.IO.File.Delete(org_fn);
            }
            //-----------------
            Banner.ImageName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
            //-----------------
            string ImagePath;
            ImagePath = Directory.GetCurrentDirectory() + "\\wwwroot\\images\\banners\\" + Banner.ImageName;

            using (var stream = new FileStream(ImagePath, FileMode.Create))
            {
                ImageFile.CopyTo(stream);
            }

        }
        //--------------------------------------------
        _context.Attach(Banner).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BannerExists(Banner.Id))
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

    private bool BannerExists(int id)
    {
        return _context.Banners.Any(e => e.Id == id);
    }
}
