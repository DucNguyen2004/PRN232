using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using DTOs;

namespace FE_PRN232Project.Services
{
    public class CustomAuthorizationMessageHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        public CustomAuthorizationMessageHandler(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7041/") }; // Replace with API URL
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Attach Access Token
            var accessToken = await _localStorage.GetItemAsync<string>("accessToken");
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    // Attempt refresh
                    var refreshResponse = await _httpClient.PostAsJsonAsync("api/auth/refresh", refreshToken);

                    if (refreshResponse.IsSuccessStatusCode)
                    {
                        var newToken = await refreshResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
                        if (newToken is not null)
                        {
                            // Save new tokens
                            await _localStorage.SetItemAsync("accessToken", newToken.AccessToken);
                            await _localStorage.SetItemAsync("refreshToken", newToken.RefreshToken);

                            // Retry original request with new token
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken.AccessToken);
                            return await base.SendAsync(request, cancellationToken);
                        }
                    }
                }
            }

            return response;
        }
    }
}
