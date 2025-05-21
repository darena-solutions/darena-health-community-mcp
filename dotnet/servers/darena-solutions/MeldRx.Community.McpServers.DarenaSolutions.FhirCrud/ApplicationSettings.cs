using Duende.IdentityServer.Models;

namespace MeldRx.Community.McpServers.DarenaSolutions.FhirCrud;

public class ApplicationSettings
{
    public string? DatabaseConStr { get; set; }

    public bool MigrateDatabase { get; set; }

    public bool UseIdentityServerAuthentication { get; set; }

    public List<Client> DefaultClients { get; set; } = [];
}
