using Microsoft.AspNetCore.Mvc;
using MovieServiceContracts;

namespace MoviePresentation.Controllers
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
            await serviceManager.Actors.AddActorToMovieAsync(movieId, actorId);

            return NoContent();
        }
    }
}
