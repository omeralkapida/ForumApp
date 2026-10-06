using ForumApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForumApp.Data.Configurations
{
    public class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Content)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Topics)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Comments)
                .WithOne(x => x.Topic)
                .HasForeignKey(x => x.TopicId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
