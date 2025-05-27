using CommandLine;
using ModelContextProtocol.Client;

namespace MeldRx.Community.PrValidator.Commands;

[Verb("verify-list-tools", HelpText = "Verifies that an MCP server can list tools.")]
public class VerifyListToolsCommand : ICommand
{
    [Option('c', "changed-files", HelpText = "A list of files that were changed")]
    public IEnumerable<string> ChangedFiles { get; set; } = [];

    [Option(
        'l',
        "language",
        HelpText = "The programming language being used",
        Default = ProgrammingLanguage.Net
    )]
    public ProgrammingLanguage Language { get; set; } = ProgrammingLanguage.Net;
}

public class VerifyListToolsCommandHandler : ICommandHandler<VerifyListToolsCommand>
{
    private const string McpServerEndpoint = "http://localhost:3056";
    private readonly HttpClient _httpClient = new HttpClient();

    public async Task<bool> HandleAsync(VerifyListToolsCommand command)
    {
        if (
            !FileUtilities.TryGetProjectDirectories(
                command.ChangedFiles.ToList(),
                out var projectDirectories
            )
        )
        {
            return false;
        }

        foreach (var projectDir in projectDirectories)
        {
            Console.WriteLine($"Building and testing project directory: {projectDir}");

            try
            {
                await SetupDockerAsync(command.Language, projectDir);
                if (!await TryWaitUntilServerIsReadyAsync(projectDir))
                {
                    return false;
                }

                await using (var mcpClient = await CreateClientAsync())
                {
                    var tools = await mcpClient.ListToolsAsync();
                    if (tools.Count == 0)
                    {
                        ConsoleUtilities.WriteErrorLine(
                            $"ERROR: No tools were returned by the MCP server at project location: {projectDir}"
                        );

                        return false;
                    }

                    var toolNames = tools.Select(x => x.Name).ToList();
                    Console.WriteLine(
                        $"SUCCESS: Tools returned: {string.Join(" | ", toolNames)}. Project: {projectDir}"
                    );
                }

                await CleanupDockerAsync();
            }
            catch (ProcessHelperException exception)
            {
                ConsoleUtilities.WriteErrorLine(exception.Message);
                return false;
            }
        }

        return true;
    }

    private async Task<IMcpClient> CreateClientAsync()
    {
        var transportOptions = new SseClientTransportOptions
        {
            Endpoint = new Uri(McpServerEndpoint),
            UseStreamableHttp = true,
        };

        var clientTransport = new SseClientTransport(transportOptions);
        return await McpClientFactory.CreateAsync(clientTransport);
    }

    private async Task<bool> TryWaitUntilServerIsReadyAsync(string projectDir)
    {
        var retries = 0;

        while (true)
        {
            try
            {
                var url = $"{McpServerEndpoint}/hello-world";
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch
            {
                retries++;
                if (retries == 3)
                {
                    ConsoleUtilities.WriteErrorLine(
                        $"Attempted /hello-world 3 times without success for project: {projectDir}"
                    );

                    return false;
                }

                await Task.Delay(1500);
            }
        }
    }

    private async Task SetupDockerAsync(ProgrammingLanguage language, string projectDir)
    {
        var workingDirectory = Path.Combine(
            FileUtilities.GetRepoRootDirectory(),
            language == ProgrammingLanguage.Net ? DirectoryNames.Dotnet : DirectoryNames.Typescript
        );

        using (
            var dockerBuildProcess = new ProcessHelper(
                "docker",
                $"build -t mcpserver -f {Path.Combine(projectDir, "Dockerfile")} .",
                workingDirectory
            )
        )
        {
            await dockerBuildProcess.RunAsync();
        }

        using var dockerRunProcess = new ProcessHelper(
            "docker",
            "run --rm --name TestMcpServer -d -p 3056:5000 mcpserver:latest",
            workingDirectory
        );

        await dockerRunProcess.RunAsync();
    }

    private async Task CleanupDockerAsync()
    {
        using (
            var containerStopProcess = new ProcessHelper("docker", "container stop TestMcpServer")
        )
        {
            await containerStopProcess.RunAsync();
        }

        using var imageRemoveProcess = new ProcessHelper("docker", "image rm mcpserver:latest");
        await imageRemoveProcess.RunAsync();
    }
}
