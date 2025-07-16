using MovieCore.DomainContracts;
using MovieServiceContracts;

namespace MovieServices
{
    public class ServiceManager : IServiceManager
    {
        public IMovieService Movies { get; }
        public ServiceManager(IUnitOfWork uow)
        {
            Movies = new MovieService(uow);
            // Actors = new ActorService(uow);
            // Reviews = new ReviewService(uow);
        }

    }
}
