using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.Entities;

public class Customer : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public int LoyaltyPoints { get; set; } = 0;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
    public CustomerPreferences? Preferences { get; set; }
}
