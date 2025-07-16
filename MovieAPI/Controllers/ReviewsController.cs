using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCore.DomainContracts;
using MovieCore.Models.Dtos;
using MovieData.Data;

namespace MovieAPI.Controllers
{
    [Route("api/movies/{movieId}/reviews")]
    [ApiController]
    [Produces("application/json")]
    public class ReviewsController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public ReviewsController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        // GET: api/movies/{movieId}/reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsForMovie(int movieId)
        {
            var movieExists = await uow.ReviewRepository.MovieExistsAsync(movieId);
            if (!movieExists)
                return NotFound($"Movie with ID {movieId} was not found.");

            var reviews = await uow.ReviewRepository.GetByMovieIdAsync(movieId);

            var result = reviews.Select(r => new ReviewDto(r.Id, r.ReviewerName, r.Comment, r.Rating));

            return Ok(result);
        }
    }
}
