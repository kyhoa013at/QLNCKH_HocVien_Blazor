using System.Net.Http.Json;
using QLNCKH_HocVien.Client.Models;

namespace QLNCKH_HocVien.Client.Services
{
    public class HoiDongService
    {
        private readonly HttpClient _http;

        public HoiDongService(HttpClient http)
        {
            _http = http;
        }

        // --- API HỘI ĐỒNG ---
        public async Task<List<HoiDong>> LayDsHoiDong()
            => await _http.GetFromJsonAsync<List<HoiDong>>("api/HoiDong") ?? new();

        public async Task TaoHoiDong(HoiDong hd)
        {
            var res = await _http.PostAsJsonAsync("api/HoiDong", hd);
            if (!res.IsSuccessStatusCode)
            {
                var err = await res.Content.ReadAsStringAsync();
                throw new Exception(err);
            }
        }

        public async Task XoaHoiDong(int id)
        {
            var res = await _http.DeleteAsync($"api/HoiDong/{id}");

            // THÊM DÒNG NÀY: Nếu Server lỗi, nó sẽ ném Exception để giao diện hiện thông báo đỏ
            if (!res.IsSuccessStatusCode)
            {
                var err = await res.Content.ReadAsStringAsync();
                throw new Exception($"Không thể xóa: {err}");
            }
        }

        // --- DỮ LIỆU THAM CHIẾU ---
        public async Task<List<ChuyenDeNCKH>> LayDsChuyenDe()
            => await _http.GetFromJsonAsync<List<ChuyenDeNCKH>>("api/ChuyenDeNCKH") ?? new();

        public async Task<List<GiaoVien>> LayDsGiaoVien()
            => await _http.GetFromJsonAsync<List<GiaoVien>>("api/GiaoVien") ?? new();

        // Gọi thêm API Đơn vị, Chức vụ từ 6pg.org để hiển thị chi tiết như yêu cầu
        public async Task<List<ToChuc>> LayDsToChuc()
        {
            var res = await _http.GetFromJsonAsync<ApiResponse<List<ToChuc>>>("http://apidanhmuc.6pg.org/api/tochuc/getall");
            return res?.Data ?? new List<ToChuc>();
        }

        public async Task<List<ChucVu>> LayDsChucVu()
            => (await _http.GetFromJsonAsync<ApiResponse<List<ChucVu>>>("http://apidanhmuc.6pg.org/api/chucvu/getall"))?.Data ?? new();
    }
}