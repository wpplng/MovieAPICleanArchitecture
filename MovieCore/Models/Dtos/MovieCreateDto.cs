using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.Dtos
{
    public class MovieCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; } = string.Empty;
        [Range(1888, 2100, ErrorMessage = "Must be between the years of 1888 to 2100")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Genre is required")]
        [StringLength(50, ErrorMessage = "Genre cannot be longer than 50 characters")]
        public string Genre { get; set; } = string.Empty;
        [Range(1, 500, ErrorMessage = "Must be between 1 and 500 minutes")]
        public int Duration { get; set; }
    }
}
