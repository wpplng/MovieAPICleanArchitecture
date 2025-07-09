using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.Models.Dtos;
using MovieAPI.Models.Entities;

namespace MovieAPI.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext context;

        public MoviesController(MovieContext context)
        {
            this.context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies([FromQuery] string? genre, [FromQuery] int? year)
        {
            var query = context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(m => m.Genre.ToLower() == genre.ToLower());
            }

            if (year.HasValue)
            {
                query = query.Where(m => m.Year == year);
            }

            var movies = await query
                .Select(m => new MovieDto(m.Id, m.Title, m.Year, m.Genre, m.Duration))
                .ToListAsync();

            return Ok(movies);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await context.Movies
                .Where(m => m.Id == id)
                .Select(m => new MovieDto
                            (m.Id, m.Title, m.Year, m.Genre, m.Duration))
                .FirstOrDefaultAsync();

            if (movie == null) return NotFound($"Movie with ID {id} was not found.");

            return Ok(movie);
        }

        // GET: api/Movies/5/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movie = await context.Movies
                .Where(m => m.Id == id)
                .Select(m => new MovieDetailDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    Genre = m.Genre,
                    Duration = m.Duration,
                    Synopsis = m.MovieDetails.Synopsis,
                    Language = m.MovieDetails.Language,
                    Budget = m.MovieDetails.Budget,
                    Reviews = m.Reviews.Select(r => new ReviewDto
                                                    (r.Id, r.ReviewerName, r.Comment, r.Rating))
                                                    .ToList(),
                    Actors = m.Actors.Select(a => new ActorDto
                                                  (a.Id, a.Name, a.BirthYear))
                                                  .ToList()
                })
                .FirstOrDefaultAsync();

            if (movie == null) return NotFound($"Movie with ID {id} was not found.");

            return Ok(movie);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieUpdateDto dto)
        {
            var movie = await context.Movies.FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound($"Movie with ID {id} was not found.");

            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Genre = dto.Genre;
            movie.Duration = dto.Duration;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound($"Movie with ID {id} was not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie(MovieCreateDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Year = dto.Year,
                Genre = dto.Genre,
                Duration = dto.Duration,
            };

            context.Movies.Add(movie);
            await context.SaveChangesAsync();

            var movieDto = new MovieDto
                (movie.Id, movie.Title, movie.Year, movie.Genre, movie.Duration);

            return CreatedAtAction(nameof(GetMovie), new { id = movieDto.Id }, movieDto);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await context.Movies.FindAsync(id);

            if (movie == null) return NotFound($"Movie with ID {id} was not found.");

            context.Movies.Remove(movie);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return context.Movies.Any(e => e.Id == id);
        }
    }
}
