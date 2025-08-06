namespace QuokkaServiceRegistry.Services;

public class VersionService : IVersionService
{
    public string Version { get; }
    public string CommitHash { get; }
    public string BuildDate { get; }

    public VersionService()
    {
        // These will be set via environment variables during Docker build
        Version = Environment.GetEnvironmentVariable("APP_VERSION") ?? "dev";
        CommitHash = Environment.GetEnvironmentVariable("APP_COMMIT_HASH") ?? "unknown";
        BuildDate = Environment.GetEnvironmentVariable("APP_BUILD_DATE") ?? DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");
    }

    public string GetFullVersionString()
    {
        return $"v{Version} ({CommitHash[..Math.Min(7, CommitHash.Length)]})";
    }
}