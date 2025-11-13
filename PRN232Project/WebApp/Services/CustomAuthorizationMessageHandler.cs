using Blazored.LocalStorage;
using DTOs;
using System.Net;
using System.Net.Http.Headers;

namespace WebApp.Services
{
    public class CustomAuthorizationMessageHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomAuthorizationMessageHandler(ILocalStorageService localStorage, IHttpClientFactory httpClientFactory)
        {
            _localStorage = localStorage;
            _httpClientFactory = httpClientFactory;
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
                    var refreshClient = _httpClientFactory.CreateClient("API");

                    var refreshResponse = await refreshClient.PostAsJsonAsync(
                        "api/auth/refresh",
                        new { refreshToken },
                        cancellationToken);

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
