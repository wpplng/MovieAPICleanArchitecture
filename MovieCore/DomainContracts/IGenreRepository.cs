using MovieCore.Models.Entities;

namespace MovieCore.DomainContracts
{
    public interface IGenreRepository
    {
        Task<Genre?> GetAsync(int id);
    }
}
