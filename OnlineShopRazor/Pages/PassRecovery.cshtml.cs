using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace OnlineShopRazor.Pages
{
    public class PassRecoveryModel : PageModel
    {
        private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

        public PassRecoveryModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
        {
            _context = context;

        }

        [BindProperty, Required]
        public string? Email { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ////-------------------------------------------

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(Email);
            if (!match.Success)
            {
                ModelState.AddModelError("Email", "Email is not valid");
                return Page();
            }

            ////-------------------------------------------

            var foundUser = _context.Users.FirstOrDefault(x => x.Email == Email.Trim());
            if (foundUser == null)
            {
                ModelState.AddModelError("Email", "Email is not exist");
                return Page();
            }

            ////-------------------------------------------

            foundUser.RecoveryCode = new Random().Next(10000, 100000);
            _context.Users.Update(foundUser);
            _context.SaveChanges();

            ////-------------------------------------------

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("nets.devs@gmail.com");
            mail.To.Add(foundUser.Email);
            mail.Subject = "Recovery code";
            mail.Body = "your recovery code is: " + foundUser.RecoveryCode;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("nets.devs@gmail.com", "gmaj evef xyjj kwik");
            SmtpServer.EnableSsl = true;

            await SmtpServer.SendMailAsync(mail);

            ////-------------------------------------------
            return Redirect("/ResetPassword?email=" + foundUser.Email);
        }
    }
}
