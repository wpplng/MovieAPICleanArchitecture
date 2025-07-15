using MovieCore.DomainContracts;
using MovieData.Data;

namespace MovieData.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IMovieRepository MovieRepository { get; }
        public IReviewRepository ReviewRepository { get; }
        public IActorRepository ActorRepository { get; }

        private readonly MovieContext context;

        public UnitOfWork(MovieContext context)
        {
            MovieRepository = new MovieRepository(context);
            ReviewRepository = new ReviewRepository(context);
            ActorRepository = new ActorRepository(context);
            this.context = context;
        }

        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
