namespace Infrastructure.Configuration;

public class InfrastructureConfiguration
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseProvider { get; set; } = "SqlServer";
    public int CommandTimeout { get; set; } = 30;
    public bool EnableSensitiveDataLogging { get; set; } = false;
}
