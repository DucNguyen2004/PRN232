using Blazored.LocalStorage;
using DTOs;

namespace WebApp.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly ApiAuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient http, ILocalStorageService localStorage, ApiAuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> Login(string email, string password)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            if (result is null)
                return false;

            await _localStorage.SetItemAsync("accessToken", result.AccessToken);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);
            await _localStorage.SetItemAsync("userId", result.UserId);

            _authStateProvider.NotifyUserAuthentication(result.AccessToken);

            return true;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("accessToken");
            await _localStorage.RemoveItemAsync("refreshToken");
            _authStateProvider.NotifyUserLogout();
        }

        public async Task<string?> GetAccessToken() =>
            await _localStorage.GetItemAsync<string>("accessToken");
    }
}
