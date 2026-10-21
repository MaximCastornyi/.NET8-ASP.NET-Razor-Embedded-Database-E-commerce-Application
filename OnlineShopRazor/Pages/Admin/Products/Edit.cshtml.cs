using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Products;

[Authorize(Roles = "admin")]
public class EditModel : PageModel
{
    private readonly OnlineDataBaseContext _context;

    public EditModel(OnlineDataBaseContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    public List<ProductGalery> Gallery { get; set; }



    public async Task<IActionResult> OnPostDeleteGallery(int id)
    {
        var gallery = await _context.ProductGaleries.FirstOrDefaultAsync(x => x.Id == id);
        //--------------------------------------------
        if (gallery == null)
        {
            return NotFound();
        }
        //--------------------------------------------
        // Delete the image file (if it exists)
        string d = Directory.GetCurrentDirectory();
        string path = d + "\\wwwroot\\images\\products\\" + gallery.ImageName;
        //----------
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
        //--------------------------------------------
        // Remove the gallery entry from the database
        _context.ProductGaleries.Remove(gallery);
        await _context.SaveChangesAsync();
        //--------------------------------------------
        return RedirectToPage("Edit", new { id = gallery.ProductId });
    }



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
        Product = product;
        //------Loading Gallery-----------
        Gallery = await _context.ProductGaleries.
                        Where(x => x.ProductId == Product.Id).ToListAsync();
        //--------------------------------
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync(IFormFile? MainImage, IFormFile[]? GalleryImages)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        //================saving image=====================
        //----------main image---------
        if (MainImage != null)
        {
            string d = Directory.GetCurrentDirectory();
            string fn = d + "\\wwwroot\\images\\products\\";

            string mainImagePath = fn + Product.ImageName;

            if (System.IO.File.Exists(mainImagePath))
            {
                System.IO.File.Delete(mainImagePath);
            }

            Product.ImageName = Guid.NewGuid() + Path.GetExtension(MainImage.FileName);
            //------------------------------------------------
            fn = d + "\\wwwroot\\images\\products\\" + Product.ImageName;
            //------------------------------------------------

            using (var stream = new FileStream(fn, FileMode.Create))
            {
                MainImage.CopyTo(stream);
            }

        }
        //------------------------------------------------
        //------------saving gallery images----------------
        if (GalleryImages != null)
        {
            foreach (var item in GalleryImages)
            {
                var newGallery = new ProductGalery();
                newGallery.ProductId = Product.Id;
                newGallery.ImageName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                //------------------------------------------------
                string d = Directory.GetCurrentDirectory();
                string fn = d + "\\wwwroot\\images\\products\\" + newGallery.ImageName;
                //------------------------------------------------
                using (var stream = new FileStream(fn, FileMode.Create))
                {
                    item.CopyTo(stream);
                }
                //------------------------------------------------
                _context.ProductGaleries.Add(newGallery);
            }
        }
        //=================================================
        _context.Attach(Product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(Product.Id))
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

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}
