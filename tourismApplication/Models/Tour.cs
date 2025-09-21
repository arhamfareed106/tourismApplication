using System.ComponentModel.DataAnnotations;

namespace tourismApplication.Models
{
    public class Tour
    {
        public int TourId { get; set; }

        // We use Title to store the selected location name.
        [Required, StringLength(150)]
        public string Title { get; set; } = string.Empty;

        // Not shown on the planner UI, but ok to keep:
        [StringLength(600)]
        public string? Description { get; set; }

        [Range(1, 30, ErrorMessage = "Duration (days) must be 1–30.")]
        [Display(Name = "Duration (days)")]
        public int DurationDays { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Available From")]
        public DateTime AvailableFrom { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Available To")]
        public DateTime AvailableTo { get; set; }

        [Range(1, 200)]
        [Display(Name = "People")]
        public int PeopleCount { get; set; }

        [Range(0, 99999)]
        [Display(Name = "Price / Person / Day")]
        public decimal PricePerDay { get; set; }

        [Range(0, 9999999)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Range(1, 200, ErrorMessage = "Group size limit must be 1–200.")]
        [Display(Name = "Group Size Limit")]
        public int GroupSizeLimit { get; set; }


        ////Images
        [Url]
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }
    }
}
