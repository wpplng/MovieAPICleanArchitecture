using Microsoft.EntityFrameworkCore;
using MovieCore.Models.Entities;
using MovieData.Configurations;

namespace MovieData.Data
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; } = default!;
        public DbSet<Actor> Actors { get; set; } = default!;
        public DbSet<Review> Reviews { get; set; } = default!;
        public DbSet<Genre> Genres { get; set; } = default!;
        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MovieConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());
            modelBuilder.Entity<MovieDetails>()
                .ToTable("MovieDetails")
                .Property(m => m.Budget)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Actor>()
                .ToTable("Actor");
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Comedy" },
                new Genre { Id = 3, Name = "Documentary" }
            );
        }
    }
}
