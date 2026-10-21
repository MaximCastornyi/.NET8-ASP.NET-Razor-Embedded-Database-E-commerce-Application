using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Settings;

[Authorize(Roles = "admin")]
public class IndexModel : PageModel
{
    private readonly OnlineDataBaseContext _context;

    public IndexModel(OnlineDataBaseContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Setting Setting { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        var setting = await _context.Settings.FirstOrDefaultAsync();

        // Если записи еще нет в базе данных, автоматически создаем дефолтную
        if (setting == null)
        {
            setting = new Setting
            {
                Title = "My Shop",
                Shipping = 0
            };

            _context.Settings.Add(setting);
            await _context.SaveChangesAsync();
        }

        Setting = setting;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(IFormFile? newLogo)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (newLogo != null)
        {
            // Формируем кроссплатформенный путь к папке wwwroot/images
            string imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            // Если старый логотип существовал, удаляем файл
            if (!string.IsNullOrEmpty(Setting.Logo))
            {
                string prevLogoPath = Path.Combine(imagesFolder, Setting.Logo);
                if (System.IO.File.Exists(prevLogoPath))
                {
                    System.IO.File.Delete(prevLogoPath);
                }
            }

            // Генерируем уникальное имя файла
            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(newLogo.FileName);
            string fullPath = Path.Combine(imagesFolder, newFileName);

            // Асинхронно сохраняем файл
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await newLogo.CopyToAsync(stream);
            }

            Setting.Logo = newFileName;
        }

        _context.Attach(Setting).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SettingExists(Setting.Id))
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

    private bool SettingExists(int id)
    {
        return _context.Settings.Any(e => e.Id == id);
    }
}