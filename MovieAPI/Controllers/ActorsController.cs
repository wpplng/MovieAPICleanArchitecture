using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.Models.Entities;

namespace MovieAPI.Controllers
{
    [Route("api/movies/{movieId}/actors/{actorId}")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly MovieContext context;

        public ActorsController(MovieContext context)
        {
            this.context = context;
        }

        // POST: api/movies/{movieId}/actors/{actorId}
        [HttpPost]
        public async Task<ActionResult> AddActorToMovie(int movieId, int actorId)
        {
            var movie = await context.Movies
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == movieId);
            if (movie == null) return NotFound($"Movie with ID {movieId} was not found.");

            var actor = await context.Actors.FindAsync(actorId);
            if (actor == null) return NotFound($"Actor with ID {actorId} was not found.");

            if (movie.Actors.Any(a => a.Id == actorId))
            {
                return Conflict($"Actor with ID {actorId} is already in this movie.");
            }

            movie.Actors.Add(actor);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
