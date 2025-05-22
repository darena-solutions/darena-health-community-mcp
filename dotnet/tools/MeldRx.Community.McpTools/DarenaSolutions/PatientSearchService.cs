using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using MeldRx.Community.Mcp.Core;
using MeldRx.Community.Mcp.Core.Models;

namespace MeldRx.Community.McpTools.DarenaSolutions;

public class PatientSearchService : IPatientSearchService
{
    public async Task<Patient?> FindByIdAsync(FhirContext context, string id)
    {
        var fhirClient = GetClient(context);
        return await fhirClient.ReadAsync<Patient>(id);
    }

    public async Task<List<Patient>> SearchByNameAsync(
        FhirContext context,
        string? firstName,
        string? lastName = null
    )
    {
        var fhirClient = GetClient(context);

        var searchParams = new SearchParams();
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            searchParams.Where($"given={firstName}");
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            searchParams.Where($"family={lastName}");
        }

        var response = await fhirClient.SearchAsync<Patient>(searchParams);
        return response?.Entry is { Count: > 0 }
            ? response.Entry.Select(x => (Patient)x.Resource).ToList()
            : [];
    }

    private FhirClient GetClient(FhirContext context)
    {
        var fhirSettings = new FhirClientSettings { PreferredFormat = ResourceFormat.Json };
        return new FhirClient(
            context.Url,
            fhirSettings,
            new FhirClientAuthMessageHandler(context.Token)
        );
    }
}
