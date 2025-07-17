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
            return Ok(movie);
        }

        // GET: api/Movies/5/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movie = await serviceManager.Movies.GetDetailsAsync(id);
            return Ok(movie);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieUpdateDto dto)
        {
            await serviceManager.Movies.UpdateAsync(id, dto);
            return NoContent();
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie(MovieCreateDto dto)
        {
            var movie = await serviceManager.Movies.CreateAsync(dto);
            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // POST: api/Movies/5/details
        [HttpPost("{id}/details")]
        public async Task<IActionResult> AddOrUpdateMovieDetails(int id, MovieDetailsCreateOrUpdateDto dto)
        {
            await serviceManager.Movies.AddOrUpdateDetailsAsync(id, dto);
            return NoContent();
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            await serviceManager.Movies.DeleteAsync(id);
            return NoContent();
        }
    }
}
