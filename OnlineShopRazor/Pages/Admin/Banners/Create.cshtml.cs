using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Banners;

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
    public Banner Banner { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync(IFormFile? ImageFile)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        //--------------Saving image----
        if (ImageFile != null)
        {
            Banner.ImageName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(ImageFile.FileName);
            string fn;
            fn = Directory.GetCurrentDirectory();
            string ImagePath = fn + "\\wwwroot\\images\\banners\\" + Banner.ImageName;

            using (var stream = new FileStream(ImagePath, FileMode.Create))
            {
                ImageFile.CopyTo(stream);
            }
        }
        //------------------------------
        _context.Banners.Add(Banner);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
