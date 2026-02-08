namespace ECommerce.Models.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string resource, string key) : base($"{resource} with key '{key}' was not found")
    {
    }
}

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(string message, List<string> errors) : base(message)
    {
        ValidationErrors = errors;
    }

    public List<string>? ValidationErrors { get; set; }
}

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message)
    {
    }
}

public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message)
    {
    }
}

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }
}

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}
