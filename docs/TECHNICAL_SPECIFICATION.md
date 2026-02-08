# E-Commerce Application Technical Specification

## 1. Project Architecture

### Solution Structure
```
ECommerce.sln
├── src/
│   ├── ECommerce.Web.UI/          # ASP.NET Core MVC / Razor Pages
│   ├── ECommerce.API/              # ASP.NET Core Web API
│   ├── ECommerce.Business.Logic/   # Service layer
│   ├── ECommerce.Data.Access/      # Data access (Repository pattern)
│   ├── ECommerce.Models/           # DTOs, ViewModels, Exceptions
│   ├── ECommerce.Infrastructure/   # Shared infrastructure
│   └── ECommerce.Tests/             # Unit tests
```

### Dependencies (packages.config)
```xml
Microsoft.AspNetCore.Authentication.JwtBearer 8.0.0
Microsoft.EntityFrameworkCore.SqlServer 8.0.0
Microsoft.EntityFrameworkCore.Design 8.0.0
AutoMapper 13.0.0
FluentValidation.AspNetCore 11.3.0
Serilog.AspNetCore 8.0.0
Microsoft.AspNetCore.Identity.EntityFrameworkCore 8.0.0
Swashbuckle.AspNetCore 6.5.0
```

---

## 2. Database Schema

### Users Table
```sql
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Email NVARCHAR(256) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(512) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Role NVARCHAR(50) NOT NULL DEFAULT 'Customer',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    RefreshToken NVARCHAR(256) NULL,
    RefreshTokenExpiry DATETIME2 NULL
);

CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Role ON Users(Role);
```

### Categories Table
```sql
CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    ParentId INT NULL FOREIGN KEY REFERENCES Categories(Id),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

CREATE INDEX IX_Categories_ParentId ON Categories(ParentId);
```

### Products Table
```sql
CREATE TABLE Products (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Price DECIMAL(18,2) NOT NULL,
    CategoryId INT NOT NULL FOREIGN KEY REFERENCES Categories(Id),
    ImageUrl NVARCHAR(500) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

CREATE INDEX IX_Products_Name ON Products(Name);
CREATE INDEX IX_Products_CategoryId ON Products(CategoryId);
CREATE INDEX IX_Products_Price ON Products(Price);
```

### Inventory Table
```sql
CREATE TABLE Inventory (
    ProductId UNIQUEIDENTIFIER PRIMARY KEY FOREIGN KEY REFERENCES Products(Id),
    Quantity INT NOT NULL DEFAULT 0,
    LastUpdated DATETIME2 NOT NULL DEFAULT GETDATE()
);
```

### Cart Table
```sql
CREATE TABLE Cart (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

CREATE INDEX IX_Cart_UserId ON Cart(UserId);
```

### CartItems Table
```sql
CREATE TABLE CartItems (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CartId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Cart(Id) ON DELETE CASCADE,
    ProductId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Products(Id),
    Quantity INT NOT NULL CHECK (Quantity > 0),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

CREATE INDEX IX_CartItems_CartId ON CartItems(CartId);
CREATE INDEX IX_CartItems_ProductId ON CartItems(ProductId);
```

### Orders Table
```sql
CREATE TABLE Orders (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    TotalAmount DECIMAL(18,2) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

CREATE INDEX IX_Orders_UserId ON Orders(UserId);
CREATE INDEX IX_Orders_Status ON Orders(Status);
CREATE INDEX IX_Orders_CreatedAt ON Orders(CreatedAt);
```

### OrderItems Table
```sql
CREATE TABLE OrderItems (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    OrderId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Orders(Id) ON DELETE CASCADE,
    ProductId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Products(Id),
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitPrice DECIMAL(18,2) NOT NULL
);

CREATE INDEX IX_OrderItems_OrderId ON OrderItems(OrderId);
CREATE INDEX IX_OrderItems_ProductId ON OrderItems(ProductId);
```

### Payments Table
```sql
CREATE TABLE Payments (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    OrderId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Orders(Id),
    Provider NVARCHAR(50) NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    Amount DECIMAL(18,2) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

CREATE INDEX IX_Payments_OrderId ON Payments(OrderId);
```

### Reviews Table
```sql
CREATE TABLE Reviews (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProductId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Products(Id),
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
    Rating INT NOT NULL CHECK (Rating >= 1 AND Rating <= 5),
    Comment NVARCHAR(1000) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

CREATE INDEX IX_Reviews_ProductId ON Reviews(ProductId);
CREATE INDEX IX_Reviews_UserId ON Reviews(UserId);
```

---

## 3. Frontend Pages Structure

```
src/ECommerce.Web.UI/
├── Pages/
│   ├── _ViewImports.cshtml
│   ├── _Layout.cshtml
│   ├── _ViewStart.cshtml
│   ├── Home/
│   │   ├── Index.cshtml
│   │   └── Privacy.cshtml
│   ├── Products/
│   │   ├── Index.cshtml          # Product listing with filters
│   │   └── Details.cshtml        # Product details + reviews
│   ├── Cart/
│   │   └── Index.cshtml          # Cart management
│   ├── Checkout/
│   │   └── Index.cshtml          # Checkout flow
│   ├── Account/
│   │   ├── Login.cshtml
│   │   ├── Register.cshtml
│   │   └── Profile.cshtml        # User profile & settings
│   ├── Orders/
│   │   ├── Index.cshtml          # Order history
│   │   └── Details.cshtml        # Order details
│   └── Admin/
│       ├── Dashboard.cshtml      # Admin overview
│       ├── Products.cshtml        # Product management
│       └── Inventory.cshtml       # Inventory management
├── Controllers/
│   ├── HomeController.cs
│   ├── ProductsController.cs
│   ├── CartController.cs
│   ├── CheckoutController.cs
│   ├── AccountController.cs
│   ├── OrdersController.cs
│   └── AdminController.cs
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── lib/
└── Program.cs
```

---

## 4. API Endpoints

### Authentication
```
POST   /api/auth/login         → JWT token
POST   /api/auth/register      → JWT token
POST   /api/auth/refresh       → Refresh token
```

### Products
```
GET    /api/products                    # List with pagination
GET    /api/products/{id}                # Details
GET    /api/products/paged?page=1&size=10 # Paginated
POST   /api/products                     # Create (Admin)
PUT    /api/products/{id}                # Update (Admin)
DELETE /api/products/{id}                # Delete (Admin)
```

### Categories
```
GET    /api/categories                   # List all
GET    /api/categories/{id}             # Details
POST   /api/categories                  # Create (Admin)
PUT    /api/categories/{id}            # Update (Admin)
DELETE /api/categories/{id}             # Delete (Admin)
```

### Cart
```
GET    /api/cart                        # Get cart (Auth)
POST   /api/cart/items                 # Add item (Auth)
PUT    /api/cart/items/{id}           # Update quantity (Auth)
DELETE /api/cart/items/{id}            # Remove item (Auth)
DELETE /api/cart                        # Clear cart (Auth)
```

### Orders
```
GET    /api/orders                      # User's orders (Auth)
GET    /api/orders/{id}                # Order details (Auth)
POST   /api/orders                     # Create from cart (Auth)
PUT    /api/orders/{id}/status         # Update status (Admin)
```

### Payments
```
POST   /api/payments                    # Process payment (Auth)
GET    /api/payments/order/{orderId}   # Get payment (Auth)
```

### Reviews
```
GET    /api/reviews/product/{productId}  # Product reviews
GET    /api/reviews/product/{productId}/summary # Rating summary
POST   /api/reviews                      # Create review (Auth)
DELETE /api/reviews/{id}                 # Delete review (Auth)
```

### Admin
```
GET/POST /api/admin/products            # Product CRUD (Admin)
PUT      /api/admin/inventory           # Update inventory (Admin)
GET      /api/admin/orders              # All orders (Admin)
PUT      /api/admin/orders/{id}/status  # Update order status (Admin)
```

### Error Responses
```json
400: { "success": false, "message": "Validation error", "errors": [...] }
401: { "success": false, "message": "Unauthorized" }
403: { "success": false, "message": "Forbidden" }
404: { "success": false, "message": "Resource not found" }
500: { "success": false, "message": "Internal server error" }
```

---

## 5. Entity Models

```csharp
public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public int StockQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
}

public class Order : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Status { get; set; } = "Pending";
    public decimal TotalAmount { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
```

### EF Core Configuration
```csharp
protected override void OnModelCreating(ModelBuilder mb)
{
    mb.Entity<Product>(e =>
    {
        e.HasKey(p => p.Id);
        e.Property(p => p.Name).HasMaxLength(200).IsRequired();
        e.Property(p => p.Price).HasColumnType("decimal(18,2)");
        e.HasOne(p => p.Category).WithMany().HasForeignKey(p => p.CategoryId);
        e.HasIndex(p => p.Name);
        e.HasIndex(p => p.CategoryId);
    });

    mb.Entity<Order>(e =>
    {
        e.HasKey(o => o.Id);
        e.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
        e.HasOne(o => o.User).WithMany().HasForeignKey(o => o.UserId);
        e.HasIndex(o => o.UserId);
        e.HasIndex(o => o.Status);
    });
}
```

---

## 6. Service Layer

### Interfaces
```csharp
public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(Guid id);
    Task<PagedApiResponse<List<ProductDto>>> GetProductsAsync(int page, int size, Guid? category);
    Task<ProductDto> CreateProductAsync(CreateProductDto dto);
    Task UpdateProductAsync(UpdateProductDto dto);
    Task DeleteProductAsync(Guid id);
}

public interface ICartService
{
    Task<CartDto> GetCartAsync(Guid userId);
    Task<CartDto> AddItemAsync(Guid userId, AddCartItemDto dto);
    Task<CartDto> UpdateItemAsync(Guid userId, UpdateCartItemDto dto);
    Task RemoveItemAsync(Guid userId, Guid itemId);
    Task ClearCartAsync(Guid userId);
}

public interface IOrderService
{
    Task<List<OrderSummaryDto>> GetOrdersAsync(Guid userId);
    Task<OrderDto?> GetOrderAsync(Guid orderId, Guid userId);
    Task<OrderDto> CreateOrderAsync(Guid userId);
}

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto);
}
```

### Validation
```csharp
public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.CategoryId).NotEmpty();
    }
}
```

### Exception Handling
```csharp
public class NotFoundException : Exception { public NotFoundException(string msg) : base(msg) { } }
public class BadRequestException : Exception { public BadRequestException(string msg) : base(msg) { } }
public class UnauthorizedException : Exception { public UnauthorizedException(string msg) : base(msg) { } }
```

---

## 7. Technical Implementation

### JWT Authentication
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });
```

### Dependency Injection
```csharp
// Infrastructure
builder.Services.AddDbContext<ECommerceDbContext>(opts => 
    opts.UseSqlServer(connectionString));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Business Logic
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddValidatorsFromAssemblyContaining<BusinessLogicAssemblyMarker>();
```

### Global Exception Middleware
```csharp
app.UseExceptionHandling();
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
```

### CORS Policy
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebUI", policy =>
    {
        policy.WithOrigins("https://localhost:7001")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

---

## 8. Getting Started

### Commands
```bash
# Restore packages
dotnet restore

# Create database
dotnet ef migrations add InitialCreate
dotnet ef database update

# Run API
dotnet run --project src/ECommerce.API

# Run Web UI
dotnet run --project src/ECommerce.Web.UI

# Run tests
dotnet test
```

### Configuration
Update `appsettings.json` with:
- Connection string for SQL Server
- JWT secret key
- CORS origins
