using Microsoft.EntityFrameworkCore;
using MovieCore.DomainContracts;
using MovieCore.Models.Entities;
using MovieData.Data;

namespace MovieData.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly MovieContext context;

        public ReviewRepository(MovieContext context)
        {
            this.context = context;
        }

        public async Task<bool> MovieExistsAsync(int movieId)
        {
            return await context.Movies.AnyAsync(m => m.Id == movieId);
        }

        public async Task<IEnumerable<Review>> GetByMovieIdAsync(int movieId)
        {
            return await context.Reviews
                .Where(r => r.MovieId == movieId)
                .ToListAsync();
        }
    }
}
