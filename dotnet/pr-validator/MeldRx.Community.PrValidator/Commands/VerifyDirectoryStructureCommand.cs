using CommandLine;

namespace MeldRx.Community.PrValidator.Commands;

[Verb(
    "verify-tools-dir",
    HelpText = "Verifies the directory structure of projects and solutions for MCP tools"
)]
public class VerifyDirectoryStructureCommand : ICommand
{
    [Option('c', "changed-files", HelpText = "A list of files that were changed")]
    public IEnumerable<string> ChangedFiles { get; set; } = [];
}

public class VerifyDirectoryStructureCommandHandler
    : ICommandHandler<VerifyDirectoryStructureCommand>
{
    public Task<bool> HandleAsync(VerifyDirectoryStructureCommand command)
    {
        var files = command.ChangedFiles.ToList();
        if (files.Count == 0)
        {
            return Task.FromResult(true);
        }

        var rootDir = FileUtilities.GetRepoRootDirectory();
        var hasErrors = false;
        foreach (var file in files)
        {
            if (file.StartsWith(DirectoryNames.GitHub))
            {
                continue;
            }

            if (
                file.Contains(".csproj", StringComparison.InvariantCultureIgnoreCase)
                || file.Contains(".sln", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                ConsoleUtilities.WriteErrorLine(
                    $"ERROR: Cannot contain project or solution files for MCP tools. Detected file with error: {file}"
                );

                hasErrors = true;
                continue;
            }

            var fullPath = Path.Combine(rootDir, file);
            var di = new DirectoryInfo(
                Path.GetDirectoryName(fullPath)
                    ?? throw new InvalidOperationException(
                        $"The directory of file '{file}' could not be read"
                    )
            );

            if (!FileExistsInToolsDirectory(di))
            {
                ConsoleUtilities.WriteErrorLine(
                    $"ERROR: When working with tools, all files must exist in the /{DirectoryNames.McpToolsProject} directory. "
                        + $"Detected file with error: {file}"
                );

                hasErrors = true;
            }
        }

        return Task.FromResult(!hasErrors);
    }

    private bool FileExistsInToolsDirectory(DirectoryInfo di)
    {
        if (di.Name == DirectoryNames.McpToolsProject)
        {
            return true;
        }

        return di.Parent is not null && FileExistsInToolsDirectory(di.Parent);
    }
}
