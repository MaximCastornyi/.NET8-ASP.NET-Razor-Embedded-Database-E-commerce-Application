using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;
using System.Text.RegularExpressions;

namespace OnlineShopRazor.Pages
{
    public class ShopModel : PageModel
    {
        private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

        public ShopModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get; set; } = default!;

        public async Task OnGetAsync(string searchText)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                Product = _context.Products.Where(x =>
                           EF.Functions.Like(x.Title, "%" + searchText + "%") ||
                           EF.Functions.Like(x.Tags, "%" + searchText + "%")).
                           OrderBy(x => x.Title).ToList();
            }
            else
            {
                Product = await _context.Products.OrderByDescending(x => x.Id).ToListAsync();
            }

        }
    }
}
