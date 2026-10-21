using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShopRazor.Models.Db;
using System.Text.RegularExpressions;

namespace OnlineShopRazor.Pages
{
    public class ProductDetailsModel : PageModel
    {
        private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

        public ProductDetailsModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
        {
            _context = context;
        }

        public Product? Product { get; set; }
        public List<ProductGalery> ProductGaleries { get; set; }
        public List<Product> NewProducts { get; set; }
        public List<Comment> Comments { get; set; }

        //public string Message { get; set; }
        public IActionResult OnGet(int id)
        {
            Product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (Product == null)
            {
                return NotFound();
            }
            //---------------------------
            ProductGaleries = _context.ProductGaleries.Where(x => x.ProductId == id).ToList();
            //---------------------------
            NewProducts = _context.Products.Where(x => x.Id != id).
                            Take(6).OrderByDescending(x => x.Id).ToList();
            //---------------------------
            Comments = _context.Comments.Where(x => x.ProductId == id).
                        OrderByDescending(x => x.CreateDate).ToList();
            //---------------------------
            return Page();
        }

        public async Task<IActionResult> OnPostSubmitComment(string name, string email, string comment, int productId)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(comment) && productId != 0)
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (!match.Success)
                {
                    TempData["Message"] = "Email is not valid";
                    return Redirect("/ProductDetails?id=" + productId);
                }

                Comment newComment = new Comment();
                newComment.Name = name;
                newComment.Email = email;
                newComment.CommentText = comment;
                newComment.ProductId = productId;
                newComment.CreateDate = DateTime.Now;

                _context.Comments.Add(newComment);
                _context.SaveChanges();
                TempData["Message"] = "Your comment submited successfully";
                return Redirect("/ProductDetails?id=" + productId);
            }
            else
            {
                TempData["Message"] = "Please complete your information";
                return Redirect("/ProductDetails?id=" + productId);
            }
        }
    }
}
