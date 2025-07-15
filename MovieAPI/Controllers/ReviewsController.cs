using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.Models.Dtos;

namespace MovieAPI.Controllers
{
    [Route("api/movies/{movieId}/reviews")]
    [ApiController]
    [Produces("application/json")]
    public class ReviewsController : ControllerBase
    {
        private readonly MovieContext context;

        public ReviewsController(MovieContext context)
        {
            this.context = context;
        }

        // GET: api/movies/{movieId}/reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsForMovie(int movieId)
        {
            var movieExists = await context.Movies.AnyAsync(m => m.Id == movieId);
            if (!movieExists)
                return NotFound($"Movie with ID {movieId} was not found.");

            var reviews = await context.Reviews
                .Where(r => r.MovieId == movieId)
                .Select(r => new ReviewDto(r.Id, r.ReviewerName, r.Comment, r.Rating))
                .ToListAsync();

            return Ok(reviews);
        }
    }
}
