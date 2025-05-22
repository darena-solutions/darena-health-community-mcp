using System.Reflection;

namespace MeldRx.Community.PrValidator;

public static class FileUtilities
{
    public static string GetRepoRootDirectory()
    {
        var githubWorkspaceDir = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE");
        if (!string.IsNullOrWhiteSpace(githubWorkspaceDir))
        {
            return githubWorkspaceDir;
        }

        var executingDirectory =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            ?? throw new InvalidOperationException(
                "Could not find the executing assembly location"
            );

        var currentDir = new DirectoryInfo(executingDirectory);
        while (currentDir.EnumerateDirectories().All(x => x.Name != DirectoryNames.GitHub))
        {
            currentDir =
                currentDir.Parent
                ?? throw new InvalidOperationException("Could not find root repository directory.");
        }

        return currentDir.FullName;
    }
}
