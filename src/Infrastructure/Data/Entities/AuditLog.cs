namespace Infrastructure.Data.Entities;

public enum AuditAction
{
    Insert = 1,
    Update = 2,
    Delete = 3
}

public class AuditLog : BaseEntity
{
    public string EntityName { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public AuditAction Action { get; set; }
    public string? OldValues { get; set; } // JSON
    public string? NewValues { get; set; } // JSON
    public Guid? UserId { get; set; }
}
