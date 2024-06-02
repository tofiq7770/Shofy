using System.ComponentModel.DataAnnotations;

namespace BackendMiniTask.ViewModels.Slider
{
    public class SliderCreateVM
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
    }
}
