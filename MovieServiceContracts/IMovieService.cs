using MovieCore.Models.Dtos;

namespace MovieServiceContracts
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllAsync(string? genre, int? year);
        Task<MovieDto?> GetAsync(int id);
        Task<MovieDetailDto?> GetDetailsAsync(int id);
        Task<MovieDto> CreateAsync(MovieCreateDto dto);
        Task<bool> UpdateAsync(int id, MovieUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> AddOrUpdateDetailsAsync(int id, MovieDetailsCreateOrUpdateDto dto);
    }
}
