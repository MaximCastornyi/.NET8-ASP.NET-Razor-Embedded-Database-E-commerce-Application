using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

        public IndexModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
        {
            _context = context;
        }

        public IList<Banner> Banner { get; set; } = default!;
        public IList<Product> NewProducts { get; set; } = default!;
        public IList<BestSellingFinal> BestSellingProducts { get; set; } = default!;
        public async Task OnGetAsync()
        {
            Banner = await _context.Banners.ToListAsync();
            //--------new----------------------
            NewProducts = _context.Products.OrderByDescending(p => p.Id).Take(8).ToList();
            //--------best selling-------------
            BestSellingProducts = _context.BestSellingFinals.ToList();
            //---------------------------------
        }
    }
}
