using MovieAPI.Models.Entities;

namespace MovieAPI.Models.Dtos
{
    public record MovieDto(int Id, string Title, int Year, string Genre, int Duration);
}
