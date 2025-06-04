using System.Net.Http.Headers;

namespace DarenaHealth.Community.Mcp.Core;

public class FhirClientAuthMessageHandler(string accessToken) : HttpClientHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return base.SendAsync(request, cancellationToken);
    }
}
