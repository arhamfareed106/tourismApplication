using System.ComponentModel.DataAnnotations;


namespace tourismApplication.Models;

    public class Agency
    {
        public int AgencyId { get; set; }

        [Required, StringLength(120)]
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
