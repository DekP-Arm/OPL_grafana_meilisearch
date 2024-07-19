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
        _configuration = configuration; // Initialize _configuration

        // Ensure _configuration is not null
        if (_configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
    }

    public async Task<string> RetrieveAndStoreTokenAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();

        var loginRequest = new HttpRequestMessage(HttpMethod.Post, "http://infisical-backend:8080/api/v1/auth/universal-auth/login");
        
        //var loginRequest = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8080/api/v1/auth/universal-auth/login");
       
        var loginContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("clientId", "cbc798a6-1b89-4aee-951e-9b8e4fa31b40"),
            new KeyValuePair<string, string>("clientSecret", "965044ffecdc5b4c393f590f3dcb7990737a1a049e48faca5829fb9a5c0b8c99")
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

            var secretRequest = new HttpRequestMessage(HttpMethod.Get, "http://infisical-backend:8080/api/v3/secrets/raw?workspaceId=b0839732-279f-4f2a-9de2-5e88df668672&environment=dev");
            
            // var secretRequest = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/api/v3/secrets/raw?workspaceId=b0839732-279f-4f2a-9de2-5e88df668672&environment=dev");
            
            secretRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var secretResponse = await httpClient.SendAsync(secretRequest);
            secretResponse.EnsureSuccessStatusCode();

            var content = await secretResponse.Content.ReadAsStringAsync();
            Console.WriteLine(content);

            var secretRespondDTO = JsonConvert.DeserializeObject<SecretRespondDTO>(content);

            // Extract the relevant secrets into a dictionary
            var secrets = secretRespondDTO.Secrets
                .ToDictionary(secret => secret.SecretKey, secret => secret.SecretValue);

            _configuration["MeilisearchClient:Host"] = secrets["MEILISEARCHCLIENT__HOST"];
            _configuration["MeilisearchClient:ApiKey"] = secrets["MEILISEARCHCLIENT__APIKEY"];

            Console.WriteLine(_configuration["MeilisearchClient:Host"]);
            Console.WriteLine(_configuration["MeilisearchClient:ApiKey"]);

            return secrets;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving secrets: {ex.Message}");
            throw;
        }
    }
}
