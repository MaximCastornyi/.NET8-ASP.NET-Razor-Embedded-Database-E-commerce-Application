using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.User
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

        public IndexModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
        {
            _context = context;
        }

        public OnlineShopRazor.Models.Db.User UserInfo { get; set; } = default!;

        public void OnGet()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            UserInfo = _context.Users.FirstOrDefault(x => x.Id == userId);
        }
    }
}
