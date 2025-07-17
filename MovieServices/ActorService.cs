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
            if (movie == null) return false;

            var actor = await uow.ActorRepository.GetAsync(actorId);
            if (actor == null) return false;

            if (uow.ActorRepository.ActorExistsInMovie(movie, actorId))
                return false;

            uow.ActorRepository.AddActorToMovie(movie, actor);
            await uow.CompleteAsync();
            return true;
        }
    }
}
