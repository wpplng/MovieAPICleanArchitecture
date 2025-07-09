
using Bogus;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.Models.Entities;

namespace MovieAPI.Services
{
    public class DataSeedService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<DataSeedService> logger;

        public DataSeedService(IServiceProvider serviceProvider, ILogger<DataSeedService> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();

            var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
            if (!env.IsDevelopment()) return;

            var context = scope.ServiceProvider.GetRequiredService<MovieContext>();
            if (await context.Movies.AnyAsync(cancellationToken)) return;

            try
            {
                IEnumerable<Movie> movies = GenerateMovies(10);
                await context.Movies.AddRangeAsync(movies, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                logger.LogInformation("Database seeded");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding data");
                throw;
            }

        }

        private IEnumerable<Movie> GenerateMovies(int numberOfMovies)
        {
            var actorFaker = new Faker<Actor>("en").Rules((f, a) =>
            {
                a.Name = f.Name.FullName();
                a.BirthYear = f.Date.Past(50, DateTime.Now.AddYears(-18)).Year;
            });

            var allActors = actorFaker.Generate(20);

            var reviewFaker = new Faker<Review>("en").Rules((f, r) =>
            {
                r.ReviewerName = f.Name.FullName();
                r.Comment = f.Lorem.Sentence(1);
                r.Rating = f.Random.Int(1, 5);
            });

            var movieDetailsFaker = new Faker<MovieDetails>("en").Rules((f, d) =>
            {
                d.Synopsis = f.Lorem.Paragraph();
                d.Language = f.PickRandom(new[] { "English", "Swedish", "Spanish", "French" });
                d.Budget = f.Random.Decimal(1_000_000, 200_000_000);
            });

            var movieFaker = new Faker<Movie>("en").Rules((f, m) =>
            {
                m.Title = f.Lorem.Sentence(3, 5);
                m.Year = f.Date.Past(20).Year;
                m.Genre = f.PickRandom(new[] { "Action", "Comedy", "Drama", "Horror", "Sci-Fi" });
                m.Duration = f.Random.Int(60, 180);
                m.MovieDetails = movieDetailsFaker.Generate();
                m.Reviews = reviewFaker.Generate(f.Random.Int(1, 3));
                m.Actors = f.PickRandom(allActors, f.Random.Int(2, 4)).ToList();
            });

            return movieFaker.Generate(numberOfMovies);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    }
}
