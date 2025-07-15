using System.ComponentModel.DataAnnotations;

namespace MovieCore.Models.Dtos
{
    public class MovieDetailsCreateOrUpdateDto
    {
        [Required]
        public string Synopsis { get; set; } = string.Empty;

        [Required]
        public string Language { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Budget { get; set; }
    }
}
