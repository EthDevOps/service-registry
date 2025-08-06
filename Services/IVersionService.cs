namespace QuokkaServiceRegistry.Services;

public interface IVersionService
{
    string Version { get; }
    string CommitHash { get; }
    string BuildDate { get; }
    string GetFullVersionString();
}