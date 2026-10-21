using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Products;

[Authorize(Roles = "admin")]
public class DeleteModel : PageModel
{
    private readonly OnlineDataBaseContext _context;

    public DeleteModel(OnlineDataBaseContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
        if (product is null)
        {
            return NotFound();
        }
        else
        {
            Product = product;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            //======================
            //----------------------------------------------
            string d = Directory.GetCurrentDirectory();
            string fn = d + "\\wwwroot\\images\\products\\";

            string mainImagePath = fn + product.ImageName;
            //-----------------------
            if (System.IO.File.Exists(mainImagePath))
            {
                System.IO.File.Delete(mainImagePath);
            }
            //------------------------
            var galleries = _context.ProductGaleries.Where(x => x.ProductId == id).ToList();
            if (galleries != null)
            {
                foreach (var item in galleries)
                {
                    string galleryImagePath = fn + item.ImageName;

                    if (System.IO.File.Exists(galleryImagePath))
                    {
                        System.IO.File.Delete(galleryImagePath);
                    }
                }

                _context.ProductGaleries.RemoveRange(galleries);
            }
            //--------------------------------------------------  
            //======================
            Product = product;
            _context.Products.Remove(Product);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
