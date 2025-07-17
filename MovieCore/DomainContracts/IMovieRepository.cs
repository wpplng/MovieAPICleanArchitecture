using MovieCore.Models.Entities;

namespace MovieCore.DomainContracts
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<IEnumerable<Movie>> GetFilteredAsync(string? genre, int? year);
        Task<(IEnumerable<Movie> Movies, int TotalItems)> GetPagedAsync(string? genre, int? year, int pageNumber, int pageSize);
        Task<Movie?> GetAsync(int id);
        Task<Movie?> GetWithDetailsAsync(int id);
        Task<bool> AnyAsync(int id);
        void Add(Movie movie);
        void Update(Movie movie);
        void Remove(Movie movie);
    }
}
