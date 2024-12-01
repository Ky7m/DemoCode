namespace SvitlaSmartTalk2024.Web;

public class ApiServiceClient(HttpClient httpClient)
{
    public async Task AcceptTerms(string? fullName, string? email, CancellationToken cancellationToken = default)
    {
        await httpClient.PostAsJsonAsync("acceptTerms", new { fullName, email }, cancellationToken);
    }
}
