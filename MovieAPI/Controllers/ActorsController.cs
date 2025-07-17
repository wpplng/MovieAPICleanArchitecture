using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCore.DomainContracts;
using MovieData.Data;
using MovieServiceContracts;
using MovieServices;

namespace MovieAPI.Controllers
{
    [Route("api/movies/{movieId}/actors/{actorId}")]
    [ApiController]
    [Produces("application/json")]
    public class ActorsController : ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public ActorsController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        // POST: api/movies/{movieId}/actors/{actorId}
        [HttpPost]
        public async Task<ActionResult> AddActorToMovie(int movieId, int actorId)
        {
            var success = await serviceManager.Actors.AddActorToMovieAsync(movieId, actorId);

            if (!success)
                return BadRequest("Either movie or actor not found, or actor already exists in movie.");

            return NoContent();
        }
    }
}
