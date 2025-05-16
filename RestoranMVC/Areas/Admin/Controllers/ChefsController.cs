using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestoranMVC.DAL;
using RestoranMVC.Helpers.Extensions;
using RestoranMVC.Models;
using RestoranMVC.ViewModels;
using System.Threading.Tasks;

namespace RestoranMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ChefsController : Controller
    {
        AppDbContext _context;
        private readonly IWebHostEnvironment environment;

        public ChefsController(AppDbContext context,IWebHostEnvironment environment)
        {
            _context = context;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            var chefs = _context.Chefs.ToList();
            return View(chefs);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ChefsVm chefs) 
        {
            if (!ModelState.IsValid) 
            {
                return View();
            }
            Chef chef = new Chef()
            {
                Name = chefs.Name,
                Position= chefs.Position,
            };
            if (chefs.file == null)
            {
                ModelState.AddModelError("file", "File cannot be empty");
                return View();
            }
            if (!chefs.file.ContentType.Contains("image"))
            {
                ModelState.AddModelError("file", "Choose correct file format");
                return View();
            }
            if (chefs.file.Length > 1024 * 1024 * 2)
            {
                ModelState.AddModelError("file", "Max file size is 2MB");
                return View();
            }
            chef.ImgUrl = chefs.file.CreatingFile(environment.WebRootPath, "upload");
            await _context.AddAsync(chef);
            _context.SaveChanges();
            return  RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
           var chef = _context.Chefs.FirstOrDefault(c => c.Id == id);
           if(chef == null)
            {
                return NotFound();
            }
           if(chef.ImgUrl != null)
            {
                chef.ImgUrl.DeletingFile(environment.WebRootPath, "upload");
            }
            _context.Remove(chef);
           await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var comment = _context.Chefs.FirstOrDefault(x => x.Id == id);
            if (comment == null)
            {
                return RedirectToAction("Index");
            }
            ChefsVm chefsVm = new ChefsVm
            {
                Name = comment.Name,
                Position = comment.Position,
            };
            return View(chefsVm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, ChefsVm chefsVm)
        {
            var chef = await _context.Chefs.FirstOrDefaultAsync(x => x.Id == id);
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (chefsVm.file == null)
            {
                ModelState.AddModelError("Image", "Please select an image");
                return View();
            }
            if (!chefsVm.file.ContentType.Contains("image"))
            {
                ModelState.AddModelError("image", "Please select an image!");
            }
            if (chefsVm.file.Length > 2097152)
            {
                ModelState.AddModelError("image", "Image size must be less than 2MB");
            }
            if (chefsVm.file != null)
            {
                if (chef.ImgUrl != null)
                {
                    chef.ImgUrl.DeletingFile(environment.WebRootPath, "upload");
                }
            }
            chef.Name = chefsVm.Name;
            chef.Position = chefsVm.Position;
            chef.ImgUrl = chefsVm.file.CreatingFile(environment.WebRootPath, "upload");
            _context.Chefs.Update(chef);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
