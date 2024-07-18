using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OPL_grafana_meilisearch.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class TokenService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public TokenService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<string> RetrieveAndStoreTokenAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();

        var loginRequest = new HttpRequestMessage(HttpMethod.Post, "http://backend:8090/api/v1/auth/universal-auth/login");
        var loginContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("clientId", "559801cc-0bc5-41d4-85dc-e4f530c11ebf"),
            new KeyValuePair<string, string>("clientSecret", "7474e2e185e2a120d5b2da833cf5cb398de94d20caa4ad0d766f017c16379849")
        });
        loginRequest.Content = loginContent;
        loginRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        var loginResponse = await httpClient.SendAsync(loginRequest);
        loginResponse.EnsureSuccessStatusCode();
        var authToken = await loginResponse.Content.ReadAsStringAsync();

        // Deserialize JSON response using Newtonsoft.Json
        var json = JsonConvert.DeserializeObject<dynamic>(authToken);
        string accessToken = json.accessToken;

        Console.WriteLine(accessToken);
        return accessToken;
    }

    public async Task<Dictionary<string, string>> GetSecretAsync()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var authToken = await RetrieveAndStoreTokenAsync();

            var secretRequest = new HttpRequestMessage(HttpMethod.Get, "http://backend:8090/api/v3/secrets/raw?workspaceId=b0839732-279f-4f2a-9de2-5e88df668672&environment=dev");
            secretRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var secretResponse = await httpClient.SendAsync(secretRequest);
            secretResponse.EnsureSuccessStatusCode();

            var content = await secretResponse.Content.ReadAsStringAsync();
            Console.WriteLine(content);

            var secretRespondDTO = JsonConvert.DeserializeObject<SecretRespondDTO>(content);

            // Extract the relevant secrets into a dictionary
            var secrets = secretRespondDTO.Secrets
                .ToDictionary(secret => secret.SecretKey, secret => secret.SecretValue);

            return secrets;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving secrets: {ex.Message}");
            throw;
        }
    }
}
