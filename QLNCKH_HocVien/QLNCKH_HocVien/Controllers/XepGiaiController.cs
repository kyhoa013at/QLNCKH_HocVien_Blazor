using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNCKH_HocVien.Client.Models;
using QLNCKH_HocVien.Data;

namespace QLNCKH_HocVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XepGiaiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public XepGiaiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<XepGiai>>> GetAll()
        {
            return await _context.XepGiais.OrderBy(x => x.XepHang).ToListAsync();
        }

        // API TÍNH TOÁN VÀ XẾP GIẢI TỰ ĐỘNG
        [HttpPost("process")]
        public async Task<IActionResult> ProcessRanking()
        {
            // 1. Xóa kết quả cũ
            _context.XepGiais.RemoveRange(_context.XepGiais);
            await _context.SaveChangesAsync();

            // 2. Lấy danh sách đề tài và tính điểm TB từ Phiếu chấm
            // Chỉ lấy những đề tài có điểm (đã chấm vòng chung khảo)
            var listDiem = await _context.PhieuChams
                .GroupBy(pc => pc.IdChuyenDe)
                .Select(g => new
                {
                    IdChuyenDe = g.Key,
                    DiemTB = g.Average(pc => pc.Diem)
                })
                .OrderByDescending(x => x.DiemTB) // Sắp xếp từ cao xuống thấp
                .ToListAsync();

            // 3. Thuật toán gán giải (3-5-7-10)
            var danhSachGiai = new List<XepGiai>();
            int rank = 1;

            foreach (var item in listDiem)
            {
                var giai = new XepGiai
                {
                    IdChuyenDe = item.IdChuyenDe,
                    DiemTrungBinh = Math.Round(item.DiemTB, 2),
                    XepHang = rank
                };

                if (rank <= 3) giai.TenGiai = "Giải Nhất";
                else if (rank <= 8) giai.TenGiai = "Giải Nhì";      // 3 + 5 = 8
                else if (rank <= 15) giai.TenGiai = "Giải Ba";      // 8 + 7 = 15
                else if (rank <= 25) giai.TenGiai = "Giải Khuyến Khích"; // 15 + 10 = 25
                else giai.TenGiai = "Công nhận NCKH"; // Ngoài top 25

                danhSachGiai.Add(giai);
                rank++;
            }

            // 4. Lưu vào DB
            _context.XepGiais.AddRange(danhSachGiai);
            await _context.SaveChangesAsync();

            return Ok(danhSachGiai);
        }
    }
}