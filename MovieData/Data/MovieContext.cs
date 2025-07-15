using Microsoft.EntityFrameworkCore;
using MovieData.Configurations;
using MovieCore.Models.Entities;

namespace MovieData.Data
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; } = default!;
        public DbSet<Actor> Actors { get; set; } = default!;
        public DbSet<Review> Reviews { get; set; } = default!;
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
        }
    }
}
