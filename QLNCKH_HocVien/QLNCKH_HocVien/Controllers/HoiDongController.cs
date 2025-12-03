using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNCKH_HocVien.Client.Models;
using QLNCKH_HocVien.Data;

namespace QLNCKH_HocVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoiDongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HoiDongController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<HoiDong>>> GetAll()
        {
            // Include để lấy luôn danh sách thành viên khi load
            return await _context.HoiDongs
                                 .Include(h => h.ThanhViens)
                                 .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<HoiDong>> Create(HoiDong hd)
        {
            // Logic: Một chuyên đề chỉ có 1 Hội đồng cho mỗi Vòng
            var exists = _context.HoiDongs.Any(x => x.IdChuyenDe == hd.IdChuyenDe && x.VongThi == hd.VongThi);
            if (exists) return BadRequest($"Chuyên đề này đã có hội đồng chấm {hd.VongThi} rồi!");

            _context.HoiDongs.Add(hd);
            await _context.SaveChangesAsync();
            return Ok(hd);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // 1. Tìm Hội đồng và KÈM THEO (Include) luôn danh sách thành viên
            var hd = await _context.HoiDongs
                                   .Include(x => x.ThanhViens) // Quan trọng: Load luôn con
                                   .FirstOrDefaultAsync(x => x.Id == id);

            if (hd == null) return NotFound("Không tìm thấy hội đồng này");

            // 2. Xóa danh sách thành viên (Lấy từ biến hd.ThanhViens đã load được)
            if (hd.ThanhViens != null && hd.ThanhViens.Any())
            {
                _context.ThanhVienHoiDongs.RemoveRange(hd.ThanhViens);
            }

            // 3. Xóa Hội đồng (Cha)
            _context.HoiDongs.Remove(hd);

            // 4. Lưu thay đổi (Lúc này EF tự biết thứ tự xóa con trước -> xóa cha sau)
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}