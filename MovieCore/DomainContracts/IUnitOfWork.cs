namespace MovieCore.DomainContracts
{
    public interface IUnitOfWork
    {
        IMovieRepository MovieRepository { get; }
        IReviewRepository ReviewRepository { get; }
        IActorRepository ActorRepository { get; }
        IGenreRepository GenreRepository { get; }
        Task CompleteAsync();
    }
}
