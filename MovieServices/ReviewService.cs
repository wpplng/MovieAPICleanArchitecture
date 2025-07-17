using MovieCore.DomainContracts;
using MovieCore.Models.Dtos;
using MovieServiceContracts;

namespace MovieServices
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork uow;

        public ReviewService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsForMovieAsync(int movieId)
        {
            var movieExists = await uow.ReviewRepository.MovieExistsAsync(movieId);
            if (!movieExists) return Enumerable.Empty<ReviewDto>();

            var reviews = await uow.ReviewRepository.GetByMovieIdAsync(movieId);

            return reviews.Select(r => new ReviewDto(r.Id, r.ReviewerName, r.Comment, r.Rating));
        }
    }
}
