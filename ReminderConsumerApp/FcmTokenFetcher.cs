using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class FcmTokenFetcher
{
    private readonly HttpClient _httpClient = new HttpClient();

    public async Task<string?> GetTokenForPatientAsync(int patientId)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5181/api/patient/{patientId}/fcm-token");
            request.Headers.Add("X-API-KEY", "super-secret-key"); // use the same key as your backend

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<PatientTokenResponse>();
                return json?.fcmToken;
            }
            else
            {
                Console.WriteLine($"❌ Failed to fetch token. Status: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Exception while fetching FCM token: {ex.Message}");
        }

        return null;
    }
}
public class PatientTokenResponse
{
    public int patientId { get; set; }
    public string fcmToken { get; set; } = "";
}