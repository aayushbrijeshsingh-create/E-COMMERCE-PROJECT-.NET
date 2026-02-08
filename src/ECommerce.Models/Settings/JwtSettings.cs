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
    /// Access token expiration time in minutes
    /// </summary>
    public int AccessTokenExpiryMinutes { get; set; } = 60;

    /// <summary>
    /// Refresh token expiration time in days
    /// </summary>
    public int RefreshTokenExpiryDays { get; set; } = 7;

    /// <summary>
    /// Issuer of the JWT token
    /// </summary>
    public string Issuer { get; set; } = "ECommerce API";

    /// <summary>
    /// Audience for the JWT token
    /// </summary>
    public string Audience { get; set; } = "ECommerce Clients";
}
