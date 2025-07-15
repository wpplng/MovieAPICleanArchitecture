using MovieCore.Models.Entities;

namespace MovieCore.DomainContracts
{
    public interface IReviewRepository
    {
        Task<bool> MovieExistsAsync(int movieId);
        Task<IEnumerable<Review>> GetByMovieIdAsync(int movieId);
    }
}
