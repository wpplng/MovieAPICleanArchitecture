using MovieCore.Models.Dtos;

namespace MovieServiceContracts
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetReviewsForMovieAsync(int movieId);
    }
}
