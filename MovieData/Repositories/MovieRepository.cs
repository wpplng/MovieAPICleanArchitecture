using Microsoft.EntityFrameworkCore;
using MovieCore.DomainContracts;
using MovieCore.Models.Dtos;
using MovieCore.Models.Entities;
using MovieData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Movie?> GetAsync(int id)
        {
            return await context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<bool> AnyAsync(int id)
        {
            return await context.Movies.AnyAsync(m => m.Id == id);
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
