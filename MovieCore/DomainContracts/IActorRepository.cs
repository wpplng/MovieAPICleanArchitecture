using MovieCore.Models.Entities;

namespace MovieCore.DomainContracts
{
    public interface IActorRepository
    {
        Task<Movie?> GetMovieWithActorsAsync(int movieId);
        Task<Actor?> GetAsync(int actorId);
        bool ActorExistsInMovie(Movie movie, int actorId);
        void AddActorToMovie(Movie movie, Actor actor);
    }
}
