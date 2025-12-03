using System.Net.Http.Json;
using QLNCKH_HocVien.Client.Models;

namespace QLNCKH_HocVien.Client.Services
{
    public class ChuyenDeNCKHService
    {
        private readonly HttpClient _http;

        public ChuyenDeNCKHService(HttpClient http)
        {
            _http = http;
        }

        // 1. CRUD CHUYÊN ĐỀ (API Nội bộ)
        public async Task<List<ChuyenDeNCKH>> LayDsChuyenDe()
            => await _http.GetFromJsonAsync<List<ChuyenDeNCKH>>("api/ChuyenDeNCKH") ?? new();

        public async Task ThemChuyenDe(ChuyenDeNCKH cd)
        {
            var res = await _http.PostAsJsonAsync("api/ChuyenDeNCKH", cd);
            if (!res.IsSuccessStatusCode)
            {
                var err = await res.Content.ReadAsStringAsync();
                throw new Exception(err);
            }
        }

        public async Task XoaChuyenDe(int id) => await _http.DeleteAsync($"api/ChuyenDeNCKH/{id}");

        // 2. DỮ LIỆU THAM CHIẾU (Để hiển thị)

        // Lấy DS Sinh viên (API Nội bộ) để chọn người làm chuyên đề
        public async Task<List<SinhVien>> LayDsSinhVien()
            => await _http.GetFromJsonAsync<List<SinhVien>>("api/SinhVien") ?? new();

        // Lấy Lĩnh Vực (API Ngoài)
        public async Task<List<LinhVucDeTai>> LayDsLinhVuc()
        {
            var res = await _http.GetFromJsonAsync<ApiResponse<List<LinhVucDeTai>>>("http://apidanhmuc.6pg.org/api/linhvucdetai/getall");
            return res?.Data ?? new();
        }

        // Lấy Ngành (API Ngoài - để hiển thị thông tin sinh viên: Họ tên, Lớp, Ngành)
        public async Task<List<Nganh>> LayDsNganh()
        {
            var res = await _http.GetFromJsonAsync<ApiResponse<List<Nganh>>>("http://apidanhmuc.6pg.org/api/lvnganh/getall");
            return res?.Data ?? new();
        }
    }
}