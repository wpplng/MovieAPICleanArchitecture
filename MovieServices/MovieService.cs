using MovieCore.DomainContracts;
using MovieCore.Models.Dtos;
using MovieCore.Models.Entities;
using MovieServiceContracts;

namespace MovieServices
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork uow;

        public MovieService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<MovieDto>> GetAllAsync(string? genre, int? year)
        {
            var movies = await uow.MovieRepository.GetFilteredAsync(genre, year);
            return movies.Select(m => new MovieDto(m.Id, m.Title, m.Year, m.Genre, m.Duration));
        }

        public async Task<MovieDto?> GetAsync(int id)
        {
            var movie = await uow.MovieRepository.GetAsync(id);
            if (movie == null) return null;

            return new MovieDto(movie.Id, movie.Title, movie.Year, movie.Genre, movie.Duration);
        }

        public async Task<MovieDetailDto?> GetDetailsAsync(int id)
        {
            var movie = await uow.MovieRepository.GetWithDetailsAsync(id);
            if (movie == null) return null;

            return new MovieDetailDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                Duration = movie.Duration,
                Synopsis = movie.MovieDetails?.Synopsis,
                Language = movie.MovieDetails?.Language,
                Budget = movie.MovieDetails?.Budget,
                Reviews = movie.Reviews.Select(r => new ReviewDto(r.Id, r.ReviewerName, r.Comment, r.Rating)).ToList(),
                Actors = movie.Actors.Select(a => new ActorDto(a.Id, a.Name, a.BirthYear)).ToList()
            };
        }

        public async Task<MovieDto> CreateAsync(MovieCreateDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Year = dto.Year,
                Genre = dto.Genre,
                Duration = dto.Duration
            };

            uow.MovieRepository.Add(movie);
            await uow.CompleteAsync();

            return new MovieDto(movie.Id, movie.Title, movie.Year, movie.Genre, movie.Duration);
        }

        public async Task<bool> UpdateAsync(int id, MovieUpdateDto dto)
        {
            var movie = await uow.MovieRepository.GetAsync(id);
            if (movie == null) return false;

            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Genre = dto.Genre;
            movie.Duration = dto.Duration;

            uow.MovieRepository.Update(movie);
            await uow.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movie = await uow.MovieRepository.GetAsync(id);
            if (movie == null) return false;

            uow.MovieRepository.Remove(movie);
            await uow.CompleteAsync();

            return true;
        }

        public async Task<bool> AddOrUpdateDetailsAsync(int id, MovieDetailsCreateOrUpdateDto dto)
        {
            var movie = await uow.MovieRepository.GetWithDetailsAsync(id);
            if (movie == null) return false;

            if (movie.MovieDetails == null)
            {
                movie.MovieDetails = new MovieDetails
                {
                    Synopsis = dto.Synopsis,
                    Language = dto.Language,
                    Budget = dto.Budget
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

            return true;
        }
    }
}
