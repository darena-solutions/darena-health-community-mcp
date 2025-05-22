using System.Reflection;

namespace MeldRx.Community.PrValidator;

public static class FileUtilities
{
    public static string GetExecutingDirectory()
    {
        return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            ?? throw new InvalidOperationException(
                "Could not find the executing assembly location"
            );
    }

    public static string GetRepoRootDirectory(DirectoryInfo? di = null)
    {
        var githubWorkspaceDir = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE");

        // TODO: Need to update for local as well soon
        return string.IsNullOrWhiteSpace(githubWorkspaceDir)
            ? throw new InvalidOperationException("Github workspace env variable could not be read")
            : githubWorkspaceDir;
    }
}
