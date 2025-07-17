namespace MovieServiceContracts
{
    public interface IActorService
    {
        Task<bool> AddActorToMovieAsync(int movieId, int actorId);
    }
}
