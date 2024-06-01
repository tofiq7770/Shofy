using BackendMiniTask.DAL;
using BackendMiniTask.Helpers.Extentions;
using BackendMiniTask.Models;
using BackendMiniTask.ViewModels.Slider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendMiniTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var datas = await _context.Slider.ToListAsync();
            return View(datas);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var data = await _context.Slider.FirstOrDefaultAsync(m=>m.Id==id);
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM create)
        {
            if (!ModelState.IsValid) return View();
                if (!create.Image.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Image", "File must be Image Format");
                    return View();
                }
                if (!create.Image.CheckFileSize(200))
                {

                    ModelState.AddModelError("Image", "Max File Capacity mut be 300KB");
                    return View();
                }
            
                string fileName = Guid.NewGuid().ToString() + "-" + create.Image.FileName;
                string path = Path.Combine(_env.WebRootPath, "img", fileName);
                await create.Image.SaveFileToLocalAsync(path);
                await _context.Slider.AddAsync(new Slider { Image = fileName });
                await _context.SaveChangesAsync();
            


            Slider slider = new()
            {
                Title = create.Title,
                SubTitle = create.SubTitle,
                Description = create.Description,
            };


            return RedirectToAction(nameof(Index));
        }

    }
}
