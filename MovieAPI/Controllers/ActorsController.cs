using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCore.DomainContracts;
using MovieData.Data;

namespace MovieAPI.Controllers
{
    [Route("api/movies/{movieId}/actors/{actorId}")]
    [ApiController]
    [Produces("application/json")]
    public class ActorsController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public ActorsController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        // POST: api/movies/{movieId}/actors/{actorId}
        [HttpPost]
        public async Task<ActionResult> AddActorToMovie(int movieId, int actorId)
        {
            var movie = await uow.ActorRepository.GetMovieWithActorsAsync(movieId);
            if (movie == null) return NotFound($"Movie with ID {movieId} was not found.");

            var actor = await uow.ActorRepository.GetAsync(actorId);
            if (actor == null) return NotFound($"Actor with ID {actorId} was not found.");

            if (movie.Actors.Any(a => a.Id == actorId))
            {
                return Conflict($"Actor with ID {actorId} is already in this movie.");
            }

            uow.ActorRepository.AddActorToMovie(movie, actor);
            await uow.CompleteAsync();

            return NoContent();
        }
    }
}
