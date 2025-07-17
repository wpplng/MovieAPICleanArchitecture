namespace MovieServiceContracts
{
    public interface IServiceManager
    {
        IMovieService Movies { get; }
        IActorService Actors { get; }
        IReviewService Reviews { get; }
    }
}
