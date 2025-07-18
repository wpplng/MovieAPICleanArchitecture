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

        public async Task<PagedResult<MovieDto>> GetAllAsync(string? genre, int? year, int pageNumber, int pageSize)
        {
            var (movies, totalItems) = await uow.MovieRepository.GetFilteredAsync(genre, year, pageNumber, pageSize);

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return new PagedResult<MovieDto>
            {
                Items = movies.Select(m => new MovieDto(m.Id, m.Title, m.Year, m.Genre.Name, m.Duration)),
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<MovieDto?> GetAsync(int id)
        {
            var movie = await uow.MovieRepository.GetAsync(id);
            if (movie == null) throw new KeyNotFoundException($"Movie with ID {id} was not found.");

            return new MovieDto(movie.Id, movie.Title, movie.Year, movie.Genre.Name, movie.Duration);
        }

        public async Task<MovieDetailDto?> GetDetailsAsync(int id)
        {
            var movie = await uow.MovieRepository.GetWithDetailsAsync(id);
            if (movie == null) throw new KeyNotFoundException($"Movie with ID {id} was not found.");

            return new MovieDetailDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre.Name,
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
            var genre = await uow.GenreRepository.GetAsync(dto.GenreId);
            if (genre == null)
                throw new KeyNotFoundException($"Genre with ID {dto.GenreId} was not found.");

            var titleExists = await uow.MovieRepository.ExistsWithTitleAsync(dto.Title);
            if (titleExists)
                throw new InvalidOperationException($"A movie with the title \"{dto.Title}\" already exists.");

            var movie = new Movie
            {
                Title = dto.Title,
                Year = dto.Year,
                GenreId = dto.GenreId,
                Duration = dto.Duration
            };

            uow.MovieRepository.Add(movie);
            await uow.CompleteAsync();

            return new MovieDto(movie.Id, movie.Title, movie.Year, movie.Genre.Name, movie.Duration);
        }

        public async Task<bool> UpdateAsync(int id, MovieUpdateDto dto)
        {
            var movie = await uow.MovieRepository.GetAsync(id);
            if (movie == null) throw new KeyNotFoundException($"Movie with ID {id} was not found.");

            var genre = await uow.GenreRepository.GetAsync(dto.GenreId);
            if (genre == null)
                throw new KeyNotFoundException($"Genre with ID {dto.GenreId} was not found.");

            var titleExists = await uow.MovieRepository.ExistsWithTitleAsync(dto.Title, excludeId: id);
            if (titleExists)
                throw new InvalidOperationException($"A movie with the title \"{dto.Title}\" already exists.");

            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.GenreId = dto.GenreId;
            movie.Duration = dto.Duration;

            uow.MovieRepository.Update(movie);
            await uow.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movie = await uow.MovieRepository.GetAsync(id);
            if (movie == null) throw new KeyNotFoundException($"Movie with ID {id} was not found.");

            uow.MovieRepository.Remove(movie);
            await uow.CompleteAsync();

            return true;
        }

        public async Task<bool> AddOrUpdateDetailsAsync(int id, MovieDetailsCreateOrUpdateDto dto)
        {
            var movie = await uow.MovieRepository.GetWithDetailsAsync(id);
            if (movie == null) throw new KeyNotFoundException($"Movie with ID {id} was not found.");

            if (dto.Budget < 0)
                throw new ArgumentException("Budget cannot be negative.");

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
