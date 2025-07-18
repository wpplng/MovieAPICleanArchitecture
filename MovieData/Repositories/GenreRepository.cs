using MovieCore.DomainContracts;
using MovieCore.Models.Entities;
using MovieData.Data;

namespace MovieData.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly MovieContext context;

        public GenreRepository(MovieContext context)
        {
            this.context = context;
        }

        public async Task<Genre?> GetAsync(int id)
        {
            return await context.Genres.FindAsync(id);
        }
    }
}
