using Microsoft.AspNetCore.Mvc;
using MovieCore.Models.Dtos;
using MovieServiceContracts;

namespace MoviePresentation.Controllers
{
    [Route("api/movies")]
    [ApiController]
    [Produces("application/json")]
    public class MoviesController : ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public MoviesController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies([FromQuery] string? genre, [FromQuery] int? year)
        {
            var movies = await serviceManager.Movies.GetAllAsync(genre, year);
            return Ok(movies);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await serviceManager.Movies.GetAsync(id);
            if (movie == null)
                return NotFound($"Movie with ID {id} was not found.");

            return Ok(movie);
        }

        // GET: api/Movies/5/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movie = await serviceManager.Movies.GetDetailsAsync(id);
            if (movie == null)
                return NotFound($"Movie with ID {id} was not found.");

            return Ok(movie);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieUpdateDto dto)
        {
            var success = await serviceManager.Movies.UpdateAsync(id, dto);
            if (!success)
                return NotFound($"Movie with ID {id} was not found.");

            return NoContent();
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie(MovieCreateDto dto)
        {
            var movieDto = await serviceManager.Movies.CreateAsync(dto);
            return CreatedAtAction(nameof(GetMovie), new { id = movieDto.Id }, movieDto);
        }

        // POST: api/Movies/5/details
        [HttpPost("{id}/details")]
        public async Task<IActionResult> AddOrUpdateMovieDetails(int id, MovieDetailsCreateOrUpdateDto dto)
        {
            var success = await serviceManager.Movies.AddOrUpdateDetailsAsync(id, dto);
            if (!success)
                return NotFound($"Movie with ID {id} was not found.");

            return NoContent();
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var success = await serviceManager.Movies.DeleteAsync(id);
            if (!success)
                return NotFound($"Movie with ID {id} was not found.");

            return NoContent();
        }
    }
}
