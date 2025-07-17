using Microsoft.EntityFrameworkCore;
using MovieCore.DomainContracts;
using MovieCore.Models.Entities;
using MovieData.Data;

namespace MovieData.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieContext context;

        public MovieRepository(MovieContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await context.Movies.ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetFilteredAsync(string? genre, int? year)
        {
            var query = context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(genre))
                query = query.Where(m => m.Genre.ToLower() == genre.ToLower());

            if (year.HasValue)
                query = query.Where(m => m.Year == year);

            return await query.ToListAsync();
        }

        public async Task<(IEnumerable<Movie> Movies, int TotalItems)> GetPagedAsync(string? genre, int? year, int pageNumber, int pageSize)
        {
            var query = context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(genre))
                query = query.Where(m => m.Genre.ToLower() == genre.ToLower());

            if (year.HasValue)
                query = query.Where(m => m.Year == year);

            var totalItems = await query.CountAsync();

            var movies = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (movies, totalItems);
        }

        public async Task<Movie?> GetAsync(int id)
        {
            return await context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<bool> AnyAsync(int id)
        {
            return await context.Movies.AnyAsync(m => m.Id == id);
        }

        public async Task<Movie?> GetWithDetailsAsync(int id)
        {
            return await context.Movies
                .Include(m => m.MovieDetails)
                .Include(m => m.Reviews)
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public void Add(Movie movie)
        {
            context.Movies.Add(movie);
        }

        public void Update(Movie movie)
        {
            context.Movies.Update(movie);
        }

        public void Remove(Movie movie)
        {
            context.Movies.Remove(movie);
        }
    }
}
