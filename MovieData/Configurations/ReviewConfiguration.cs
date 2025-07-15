using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCore.Models.Entities;

namespace MovieData.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Review");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.ReviewerName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(r => r.Comment)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(r => r.Rating)
                .IsRequired()
                .HasMaxLength(5);
        }
    }
}
