namespace Infrastructure.Data.Entities;

public enum ActivityType
{
    Login = 1,
    Logout = 2,
    FailedLogin = 3,
    PasswordChange = 4
}

public class UserActivity : BaseEntity
{
    public Guid? UserId { get; set; }
    public Customer? User { get; set; }
    public ActivityType ActivityType { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}
