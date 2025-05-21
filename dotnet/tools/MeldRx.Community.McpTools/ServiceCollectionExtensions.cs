using MeldRx.Community.Mcp.Core;
using MeldRx.Community.McpTools.DarenaSolutions;
using Microsoft.Extensions.DependencyInjection;

namespace MeldRx.Community.McpTools;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMcpTools(this IServiceCollection services)
    {
        return services.AddPatientAgeMcpToolss();
    }

    private static IServiceCollection AddPatientAgeMcpToolss(this IServiceCollection services)
    {
        return services
            .AddSingleton<IPatientSearchService, PatientSearchService>()
            .AddSingleton<IMcpTool, PatientAgeTool>();
    }

    private static IServiceCollection AddNonReferencedMcpTool(this IServiceCollection services)
    {
        return services;
    }
}
