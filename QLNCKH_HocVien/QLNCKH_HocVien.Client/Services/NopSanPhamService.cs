using System.Net.Http.Json;
using QLNCKH_HocVien.Client.Models;

namespace QLNCKH_HocVien.Client.Services
{
    public class NopSanPhamService
    {
        private readonly HttpClient _http;

        public NopSanPhamService(HttpClient http)
        {
            _http = http;
        }

        // 1. API NỘP SẢN PHẨM
        public async Task<List<NopSanPham>> LayDsNop()
            => await _http.GetFromJsonAsync<List<NopSanPham>>("api/NopSanPham") ?? new();

        public async Task NopBai(NopSanPham item)
        {
            var res = await _http.PostAsJsonAsync("api/NopSanPham", item);
            res.EnsureSuccessStatusCode();
        }

        public async Task XoaNop(int id) => await _http.DeleteAsync($"api/NopSanPham/{id}");

        // 2. DỮ LIỆU THAM CHIẾU (Lấy Chuyên đề & Sinh viên từ các chức năng trước)

        // Lấy danh sách chuyên đề (để chọn nộp cho cái nào)
        public async Task<List<ChuyenDeNCKH>> LayDsChuyenDe()
            => await _http.GetFromJsonAsync<List<ChuyenDeNCKH>>("api/ChuyenDeNCKH") ?? new();

        // Lấy danh sách sinh viên (để hiển thị tên người nộp)
        public async Task<List<SinhVien>> LayDsSinhVien()
            => await _http.GetFromJsonAsync<List<SinhVien>>("api/SinhVien") ?? new();
    }
}