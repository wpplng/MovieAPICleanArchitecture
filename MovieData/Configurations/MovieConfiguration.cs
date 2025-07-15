using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCore.Models.Entities;

namespace MovieData.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movie");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Title).IsRequired().HasMaxLength(100);
            builder.Property(m => m.Genre).IsRequired().HasMaxLength(50);
            builder.Property(m => m.Year).IsRequired();
            builder.Property(m => m.Duration).IsRequired().HasMaxLength(500);
            builder.HasOne(m => m.MovieDetails)
                .WithOne(md => md.Movie)
                .HasForeignKey<MovieDetails>(md => md.MovieId);
            builder.HasMany(m => m.Reviews)
                .WithOne(r => r.Movie)
                .HasForeignKey(r => r.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(m => m.Actors)
                .WithMany(a => a.Movies);
        }
    }
}
