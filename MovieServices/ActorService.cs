using MovieCore.DomainContracts;
using MovieServiceContracts;

namespace MovieServices
{
    public class ActorService : IActorService
    {
        private readonly IUnitOfWork uow;

        public ActorService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async Task<bool> AddActorToMovieAsync(int movieId, int actorId)
        {
            var movie = await uow.ActorRepository.GetMovieWithActorsAsync(movieId);
            if (movie == null) throw new KeyNotFoundException($"Movie with ID {movieId} was not found.");

            var actor = await uow.ActorRepository.GetAsync(actorId);
            if (actor == null) throw new KeyNotFoundException($"Actor with ID {actorId} was not found.");

            if (uow.ActorRepository.ActorExistsInMovie(movie, actorId))
                throw new InvalidOperationException($"Actor with ID {actorId} is already in this movie.");

            uow.ActorRepository.AddActorToMovie(movie, actor);
            await uow.CompleteAsync();
            return true;
        }
    }
}
