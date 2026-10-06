namespace ForumApp.Entities.Coomon
{
    public class BaseEntity : IBaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.CreateVersion7();
            CreatedAt = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
