using System.ComponentModel.DataAnnotations;

namespace MovieCore.Models.Dtos
{
    public class MovieUpdateDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;
        [Range(1888, 2100, ErrorMessage = "Must be between the years of 1888 to 2100")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Genre is required")]
        public string Genre { get; set; } = string.Empty;
        [Range(1, 500, ErrorMessage = "Must be between 1 and 500 minutes")]
        public int Duration { get; set; }
    }
}
