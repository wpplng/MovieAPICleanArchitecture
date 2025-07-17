using Microsoft.AspNetCore.Mvc;
using MovieCore.Models.Dtos;
using MovieServiceContracts;

namespace MoviePresentation.Controllers
{
    [Route("api/movies/{movieId}/reviews")]
    [ApiController]
    [Produces("application/json")]
    public class ReviewsController : ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public ReviewsController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        // GET: api/movies/{movieId}/reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsForMovie(int movieId)
        {
            var reviews = await serviceManager.Reviews.GetReviewsForMovieAsync(movieId);

            if (!reviews.Any())
                return NotFound($"No reviews or movie with ID {movieId} not found.");

            return Ok(reviews);
        }
    }
}
