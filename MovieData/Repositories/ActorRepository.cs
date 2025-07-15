using Microsoft.EntityFrameworkCore;
using MovieCore.DomainContracts;
using MovieCore.Models.Entities;
using MovieData.Data;

namespace MovieData.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly MovieContext context;

        public ActorRepository(MovieContext context)
        {
            this.context = context;
        }

        public async Task<Movie?> GetMovieWithActorsAsync(int movieId)
        {
            return await context.Movies
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == movieId);
        }

        public async Task<Actor?> GetAsync(int actorId)
        {
            return await context.Actors.FindAsync(actorId);
        }

        public bool ActorExistsInMovie(Movie movie, int actorId)
        {
            return movie.Actors.Any(a => a.Id == actorId);
        }

        public void AddActorToMovie(Movie movie, Actor actor)
        {
            movie.Actors.Add(actor);
        }
    }
}

