using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCore.DomainContracts;
using MovieCore.Models.Dtos;
using MovieCore.Models.Entities;
using MovieData.Data;

namespace MovieAPI.Controllers
{
    [Route("api/movies")]
    [ApiController]
    [Produces("application/json")]
    public class MoviesController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public MoviesController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies([FromQuery] string? genre, [FromQuery] int? year)
        {
            var movies = await uow.MovieRepository.GetFilteredAsync(genre, year);
            var dtos = movies.Select(m => new MovieDto
                (m.Id, m.Title, m.Year, m.Genre, m.Duration));

            return Ok(dtos);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await uow.MovieRepository.GetAsync(id);

            if (movie == null) return NotFound($"Movie with ID {id} was not found.");

            var dto = new MovieDto
                (movie.Id, movie.Title, movie.Year, movie.Genre, movie.Duration);

            return Ok(dto);
        }

        // GET: api/Movies/5/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movie = await uow.MovieRepository.GetWithDetailsAsync(id);
            if (movie == null) return NotFound($"Movie with ID {id} was not found.");

            var dto = new MovieDetailDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                Duration = movie.Duration,
                Synopsis = movie.MovieDetails?.Synopsis,
                Language = movie.MovieDetails?.Language,
                Budget = movie.MovieDetails?.Budget,
                Reviews = movie.Reviews.Select(r => new ReviewDto
                    (r.Id, r.ReviewerName, r.Comment, r.Rating)).ToList(),
                Actors = movie.Actors.Select(a => new ActorDto
                    (a.Id, a.Name, a.BirthYear)).ToList()
            };

            return Ok(dto);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieUpdateDto dto)
        {
            //var movie = await context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            var movie = await uow.MovieRepository.GetAsync(id);

            if (movie == null) return NotFound($"Movie with ID {id} was not found.");

            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Genre = dto.Genre;
            movie.Duration = dto.Duration;

            uow.MovieRepository.Update(movie);
            await uow.CompleteAsync();

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

            uow.MovieRepository.Add(movie);
            await uow.CompleteAsync();

            var movieDto = new MovieDto
                (movie.Id, movie.Title, movie.Year, movie.Genre, movie.Duration);

            return CreatedAtAction(nameof(GetMovie), new { id = movieDto.Id }, movieDto);
        }

        // POST: api/Movies/5/details
        [HttpPost("{id}/details")]
        public async Task<IActionResult> AddOrUpdateMovieDetails(int id, MovieDetailsCreateOrUpdateDto dto)
        {
            var movie = await uow.MovieRepository.GetWithDetailsAsync(id);
            if (movie == null)
                return NotFound($"Movie with ID {id} was not found.");

            if (movie.MovieDetails == null)
                {
                movie.MovieDetails = new MovieDetails
                {
                    Synopsis = dto.Synopsis,
                    Language = dto.Language,
                    Budget = dto.Budget,
                    MovieId = movie.Id
                };
            }
            else
            {
                movie.MovieDetails.Synopsis = dto.Synopsis;
                movie.MovieDetails.Language = dto.Language;
                movie.MovieDetails.Budget = dto.Budget;
            }

            uow.MovieRepository.Update(movie);
            await uow.CompleteAsync();

            return NoContent();
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await uow.MovieRepository.GetAsync(id);

            if (movie == null) return NotFound($"Movie with ID {id} was not found.");

            uow.MovieRepository.Remove(movie);
            await uow.CompleteAsync();

            return NoContent();
        }
    }
}
