using ForumApp.Entities;
using ForumApp.Entities.Coomon;
using ForumApp.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ForumApp.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        private readonly ICurrentUserService _currentUserService;
        public Guid CurrentUserId => _currentUserService.UserId;

        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }


        public DbSet<Topic> Topics => Set<Topic>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<CommentRating> CommentRatings => Set<CommentRating>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Topic>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Comment>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<CommentRating>().HasQueryFilter(x => !x.IsDeleted);
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<IBaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;

                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = DateTimeOffset.UtcNow;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
