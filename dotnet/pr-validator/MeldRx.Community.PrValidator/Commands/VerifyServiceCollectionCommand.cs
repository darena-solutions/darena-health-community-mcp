using System.Reflection;
using CommandLine;
using MeldRx.Community.Mcp.Core;
using MeldRx.Community.McpTools;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.DependencyInjection;

namespace MeldRx.Community.PrValidator.Commands;

[Verb("verify-sc", HelpText = "Verifies restrictions on ServiceCollectionExtensions.cs")]
public class VerifyServiceCollectionCommand : ICommand { }

public class VerifyServiceCollectionCommandHandler : ICommandHandler<VerifyServiceCollectionCommand>
{
    public async Task<bool> HandleAsync(VerifyServiceCollectionCommand command)
    {
        var executingPath =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            ?? throw new InvalidOperationException("Executing assembly location not found.");

        var parentDirectory = FindParentDirectory(new DirectoryInfo(executingPath));
        var workingDirectory = Path.Combine(parentDirectory.FullName, "tools");
        var solutionPath = Path.Combine(workingDirectory, "MeldRx.Community.McpTools.sln");

        if (!File.Exists(solutionPath))
        {
            throw new InvalidOperationException(
                $"The solution file at location '{solutionPath}' could not be found."
            );
        }

        var workspace = MSBuildWorkspace.Create();
        var solution = await workspace.OpenSolutionAsync(solutionPath);
        var project = solution.Projects.First(x => x.Name == "MeldRx.Community.McpTools");
        var document = project.Documents.First(x => x.Name == "ServiceCollectionExtensions.cs");
        var model =
            await document.GetSemanticModelAsync()
            ?? throw new InvalidOperationException("Semantic model couldn't be obtained");

        var syntaxRoot =
            await document.GetSyntaxRootAsync()
            ?? throw new InvalidOperationException("Syntax root couldn't be obtained");

        var methods = syntaxRoot.DescendantNodes().OfType<MethodDeclarationSyntax>();
        var privateMethods = methods.Where(x => x.Modifiers.Any(y => y.Text == "private")).ToList();
        var symbols = privateMethods.Select(x =>
            model.GetDeclaredSymbol(x)
            ?? throw new InvalidOperationException(
                $"Symbol could not be generated for method: {x.Identifier}"
            )
        );

        var hasErrors = false;
        foreach (var symbol in symbols)
        {
            if (
                !symbol.Name.StartsWith("Add")
                || (!symbol.Name.EndsWith("McpTool") && !symbol.Name.EndsWith("McpTools"))
            )
            {
                ConsoleUtilities.WriteErrorLine(
                    $"ERROR: The method {symbol.Name} does not start with 'Add' and does not end with 'McpTool' or 'McpTools'."
                );

                hasErrors = true;
                continue;
            }

            var references = (await SymbolFinder.FindReferencesAsync(symbol, solution)).ToList();
            if (references.Count == 0 || !references[0].Locations.Any())
            {
                ConsoleUtilities.WriteErrorLine(
                    $"ERROR: The method {symbol.Name} has no references"
                );
                hasErrors = true;
            }
        }

        var serviceCollection = new ServiceCollection().AddMcpTools();
        foreach (var descriptor in serviceCollection)
        {
            if (
                descriptor.ServiceType != typeof(IMcpTool)
                && !descriptor.ServiceType.IsAssignableTo(typeof(IMcpTool))
            )
            {
                continue;
            }

            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                Console.WriteLine(
                    $"ERROR: The mcp tool service '{descriptor.ServiceType.Name}' has been registered as a singleton, which "
                        + $"is not allowed."
                );

                hasErrors = true;
            }
        }

        return !hasErrors;
    }

    private DirectoryInfo FindParentDirectory(DirectoryInfo currentDirectory)
    {
        if (currentDirectory.Name == "dotnet")
        {
            return currentDirectory;
        }

        return FindParentDirectory(
            currentDirectory.Parent
                ?? throw new InvalidOperationException("Could not find the parent dotnet directory")
        );
    }
}
