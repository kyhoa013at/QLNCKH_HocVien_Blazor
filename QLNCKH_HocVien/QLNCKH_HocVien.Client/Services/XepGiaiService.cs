using System.Net.Http.Json;
using QLNCKH_HocVien.Client.Models;

namespace QLNCKH_HocVien.Client.Services
{
    public class XepGiaiService
    {
        private readonly HttpClient _http;

        public XepGiaiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<XepGiai>> LayKetQua()
            => await _http.GetFromJsonAsync<List<XepGiai>>("api/XepGiai") ?? new();

        public async Task TuDongXepGiai()
        {
            var res = await _http.PostAsync("api/XepGiai/process", null);
            res.EnsureSuccessStatusCode();
        }

        // Tham chiếu hiển thị
        public async Task<List<ChuyenDeNCKH>> LayDsChuyenDe()
            => await _http.GetFromJsonAsync<List<ChuyenDeNCKH>>("api/ChuyenDeNCKH") ?? new();

        public async Task<List<SinhVien>> LayDsSinhVien()
            => await _http.GetFromJsonAsync<List<SinhVien>>("api/SinhVien") ?? new();

        // Lấy danh sách ngành để hiển thị chi tiết (theo yêu cầu ảnh)
        public async Task<List<Nganh>> LayDsNganh()
        {
            var res = await _http.GetFromJsonAsync<ApiResponse<List<Nganh>>>("http://apidanhmuc.6pg.org/api/lvnganh/getall");
            return res?.Data ?? new();
        }
    }
}