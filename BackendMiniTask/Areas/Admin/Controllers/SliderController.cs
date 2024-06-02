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
            if (id <= 0) return BadRequest();
            var data = await _context.Slider.FirstOrDefaultAsync(m => m.Id == id);
            if (data == null) return NotFound();
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
            string path = Path.Combine(_env.WebRootPath, "assets", "images", fileName);
            await create.Image.SaveFileToLocalAsync(path);


            Slider slider = new()
            {
                Image = fileName,
                Title = create.Title,
                SubTitle = create.SubTitle,
                Description = create.Description,
            };
            await _context.Slider.AddAsync(slider);
            await _context.SaveChangesAsync();



            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Slider existed = await _context.Slider.FirstOrDefaultAsync(s => s.Id == id);

            if (existed is null) return NotFound();

            SliderUpdateVM slideVM = new()
            {
                Image = existed.Image,
                Title = existed.Title,
                SubTitle = existed.SubTitle,
                Description = existed.Description,
            };

            return View(slideVM);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, SliderUpdateVM slideVM)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            Slider existed = await _context.Slider.FirstOrDefaultAsync(s => s.Id == id);

            if (existed is null) return NotFound();

            if (slideVM.Photo is not null)
            {
                if (!slideVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Image", "File must be Image Format");
                    return View(slideVM);
                }
                if (!slideVM.Photo.CheckFileSize(200))
                {

                    ModelState.AddModelError("Image", "Max File Capacity mut be 300KB");
                    return View(slideVM);
                }
                string newImage = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "images");
                existed.Image = newImage;

            }

            existed.Title = slideVM.Title;
            existed.Description = slideVM.Description;
            existed.SubTitle = slideVM.SubTitle;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Slider slide = await _context.Slider.FirstOrDefaultAsync(s => s.Id == id);

            if (slide is null) return NotFound();

            slide.Image.DeleteFile(_env.WebRootPath, "assets", "images");

            _context.Slider.Remove(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
