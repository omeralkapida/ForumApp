using ForumApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForumApp.Data.Configurations
{
    public class CommentRatingConfiguration : IEntityTypeConfiguration<CommentRating>
    {
        public void Configure(EntityTypeBuilder<CommentRating> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Score)
                .IsRequired();

            builder.HasIndex(x => new
            {
                x.CommentId,
                x.UserId
            })
            .IsUnique();

            builder.HasOne(x => x.Comment)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.CommentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.User)
                .WithMany(x => x.CommentRatings)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
