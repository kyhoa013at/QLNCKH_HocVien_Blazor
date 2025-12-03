using QLNCKH_HocVien.Client.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace QLNCKH_HocVien.Client.Services
{
    public class SinhVienService
    {
        private readonly HttpClient _http;

        // Danh sách sinh viên giả lập (lưu trên RAM)
        private static List<SinhVien> _danhSachSinhVien = new List<SinhVien>();

        public SinhVienService(HttpClient http)
        {
            _http = http;
        }

        // --- PHẦN 1: GỌI API DANH MỤC (API THẬT) ---
        // Link API dựa trên file json.txt bạn cung cấp
        public async Task<List<Tinh>> LayDsTinh()
        {
            // Thêm http://apidanhmuc.6pg.org vào trước
            var result = await _http.GetFromJsonAsync<ApiResponse<List<Tinh>>>("http://apidanhmuc.6pg.org/api/tinh/getall");
            return result?.Data ?? new List<Tinh>();
        }

        public async Task<List<DanToc>> LayDsDanToc()
        {
            var result = await _http.GetFromJsonAsync<ApiResponse<List<DanToc>>>("http://apidanhmuc.6pg.org/api/dantoc/getall");
            return result?.Data ?? new List<DanToc>();
        }

        public async Task<List<TonGiao>> LayDsTonGiao()
        {
            var result = await _http.GetFromJsonAsync<ApiResponse<List<TonGiao>>>("http://apidanhmuc.6pg.org/api/tongiao/getall");
            return result?.Data ?? new List<TonGiao>();
        }

        public async Task<List<ChucVu>> LayDsChucVu()
        {
            var result = await _http.GetFromJsonAsync<ApiResponse<List<ChucVu>>>("http://apidanhmuc.6pg.org/api/chucvu/getall");
            return result?.Data ?? new List<ChucVu>();
        }

        public async Task<List<Nganh>> LayDsNganh()
        {
            var result = await _http.GetFromJsonAsync<ApiResponse<List<Nganh>>>("http://apidanhmuc.6pg.org/api/lvnganh/getall");
            return result?.Data ?? new List<Nganh>();
        }

        // --- PHẦN 2: QUẢN LÝ SINH VIÊN (GỌI API SERVER NỘI BỘ) ---

        public async Task<List<SinhVien>> LayTatCaSinhVien()
        {
            // Gọi vào API SinhVienController vừa tạo ở Server
            var result = await _http.GetFromJsonAsync<List<SinhVien>>("api/SinhVien");
            return result ?? new List<SinhVien>();
        }

        public async Task ThemSinhVien(SinhVien sv)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/SinhVien", sv);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Lỗi server ({response.StatusCode}): {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Ném lại lỗi HTTP để UI xử lý
                throw new Exception($"❌ Không thể kết nối tới server: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Lỗi khác (JSON parse, network...)
                throw new Exception($"❌ Lỗi không xác định: {ex.Message}");
            }
        }

        public async Task XoaSinhVien(int id)
        {
            // Gửi lệnh xóa theo ID
            await _http.DeleteAsync($"api/SinhVien/{id}");
        }
        public async Task CapNhatSinhVien(SinhVien sv)
        {
            var response = await _http.PutAsJsonAsync($"api/SinhVien/{sv.Id}", sv);
            response.EnsureSuccessStatusCode();
        }
        public string GetExportUrl() => "api/SinhVien/export";

    }
}