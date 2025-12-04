using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.JSInterop;
using QLNCKH_HocVien.Client.Models;

namespace QLNCKH_HocVien.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/Auth/login", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (result?.Success == true && !string.IsNullOrEmpty(result.Token))
            {
                await SaveToken(result.Token);
                await SaveUserInfo(result);
                SetAuthHeader(result.Token);
            }

            return result ?? new AuthResponse { Success = false, Message = "Lỗi không xác định" };
        }

        public async Task<AuthResponse> Register(RegisterRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/Auth/register", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (result?.Success == true && !string.IsNullOrEmpty(result.Token))
            {
                await SaveToken(result.Token);
                await SaveUserInfo(result);
                SetAuthHeader(result.Token);
            }

            return result ?? new AuthResponse { Success = false, Message = "Lỗi không xác định" };
        }

        public async Task Logout()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
            await _js.InvokeVoidAsync("localStorage.removeItem", "userInfo");
            _http.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string?> GetToken()
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", "authToken");
        }

        public async Task<UserInfo?> GetUserInfo()
        {
            var json = await _js.InvokeAsync<string?>("localStorage.getItem", "userInfo");
            if (string.IsNullOrEmpty(json)) return null;

            return System.Text.Json.JsonSerializer.Deserialize<UserInfo>(json);
        }

        public async Task<bool> IsAuthenticated()
        {
            var token = await GetToken();
            return !string.IsNullOrEmpty(token);
        }

        private async Task SaveToken(string token)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        }

        private async Task SaveUserInfo(AuthResponse response)
        {
            var userInfo = new UserInfo
            {
                Username = response.Username ?? "",
                Role = response.Role ?? "",
                HoTen = response.HoTen ?? "",
                IdSinhVien = response.IdSinhVien,
                IdGiaoVien = response.IdGiaoVien
            };

            var json = System.Text.Json.JsonSerializer.Serialize(userInfo);
            await _js.InvokeVoidAsync("localStorage.setItem", "userInfo", json);
        }

        private void SetAuthHeader(string token)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Gọi hàm này khi app khởi động để restore token từ localStorage
        public async Task InitializeAuth()
        {
            var token = await GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                SetAuthHeader(token);
            }
        }
    }
}