namespace ECommerce.Models.Settings;

/// <summary>
/// JWT authentication settings configuration
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Secret key for signing JWT tokens
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Token expiration time in minutes
    /// </summary>
    public int TokenExpirationMinutes { get; set; } = 60;

    /// <summary>
    /// Issuer of the JWT token
    /// </summary>
    public string Issuer { get; set; } = "ECommerce API";

    /// <summary>
    /// Audience for the JWT token
    /// </summary>
    public string Audience { get; set; } = "ECommerce Clients";
}
