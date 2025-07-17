using MovieCore.Models.Entities;

namespace MovieCore.DomainContracts
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<IEnumerable<Movie>> GetFilteredAsync(string? genre, int? year);
        Task<Movie?> GetAsync(int id);
        Task<Movie?> GetWithDetailsAsync(int id);
        Task<bool> AnyAsync(int id);
        void Add(Movie movie);
        void Update(Movie movie);
        void Remove(Movie movie);
    }
}
