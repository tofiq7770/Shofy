using System.ComponentModel.DataAnnotations;

namespace BackendMiniTask.ViewModels.Slider
{
    public class SliderUpdateVM
    {

        [Required]
        public string Title { get; set; }
        public string SubTitle { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
