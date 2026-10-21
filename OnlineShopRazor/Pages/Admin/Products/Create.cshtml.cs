using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Products;

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
    public Product Product { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync(IFormFile? MainImage, IFormFile[]? GalleryImages)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        //==============Saving Main image=======
        if (MainImage != null)
        {
            Product.ImageName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(MainImage.FileName);
            string fn;
            fn = Directory.GetCurrentDirectory();
            string ImagePath = fn + "\\wwwroot\\images\\products\\" + Product.ImageName;

            using (var stream = new FileStream(ImagePath, FileMode.Create))
            {
                MainImage.CopyTo(stream);
            }
        }
        //======================================
        _context.Products.Add(Product);
        await _context.SaveChangesAsync();
        //==============Saving Gallery images=============
        if (GalleryImages != null)
        {
            foreach (var item in GalleryImages)
            {
                var newGallery = new ProductGalery();
                newGallery.ProductId = Product.Id;
                newGallery.ImageName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(item.FileName);
                //------------------
                string fn;
                fn = Directory.GetCurrentDirectory();
                string ImagePath = fn + "\\wwwroot\\images\\products\\" + newGallery.ImageName;

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    item.CopyTo(stream);
                }
                //------------------
                _context.ProductGaleries.Add(newGallery);
            }
        }
        await _context.SaveChangesAsync();
        //================================================
        return RedirectToPage("./Index");
    }
}
