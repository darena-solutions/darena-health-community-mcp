using MeldRx.Community.Mcp.Core;
using MeldRx.Community.McpTools.DarenaSolutions;
using Microsoft.Extensions.DependencyInjection;

namespace MeldRx.Community.McpTools;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMcpTools(this IServiceCollection services)
    {
        return services.AddPatientAgeMcpTool();
    }

    private static IServiceCollection AddPatientAgeMcpTool(this IServiceCollection services)
    {
        return services
            .AddSingleton<IPatientSearchService, PatientSearchService>()
            .AddScoped<IMcpTool, PatientAgeTool>();
    }
}
