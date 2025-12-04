using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNCKH_HocVien.Client.Models;
using QLNCKH_HocVien.Data;
using QLNCKH_HocVien.Models;
using System.Security.Claims;

namespace QLNCKH_HocVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // ✅ Yêu cầu đăng nhập cho toàn bộ controller
    public class ChuyenDeNCKHController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChuyenDeNCKHController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ChuyenDeNCKH>>> GetAll()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            // ✅ PHÂN QUYỀN: Sinh viên chỉ xem chuyên đề của mình
            if (role == AppRoles.SinhVien)
            {
                var idSinhVienClaim = User.FindFirst("IdSinhVien")?.Value;
                if (string.IsNullOrEmpty(idSinhVienClaim) || !int.TryParse(idSinhVienClaim, out int idSinhVien))
                {
                    return BadRequest("Không tìm thấy thông tin sinh viên");
                }

                // Chỉ trả về chuyên đề của sinh viên này
                return await _context.ChuyenDeNCKHs
                    .Where(x => x.IdHocVien == idSinhVien)
                    .ToListAsync();
            }

            // ✅ Giáo viên và Admin thấy tất cả
            return await _context.ChuyenDeNCKHs.ToListAsync();
        }

        [HttpPost]
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.SinhVien}")] // Chỉ Admin và SinhVien mới tạo được
        public async Task<ActionResult<ChuyenDeNCKH>> Create(ChuyenDeNCKH cd)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            // ✅ Nếu là sinh viên, chỉ cho phép tạo chuyên đề cho chính mình
            if (role == AppRoles.SinhVien)
            {
                var idSinhVienClaim = User.FindFirst("IdSinhVien")?.Value;
                if (string.IsNullOrEmpty(idSinhVienClaim) || !int.TryParse(idSinhVienClaim, out int idSinhVien))
                {
                    return BadRequest("Không tìm thấy thông tin sinh viên");
                }

                // Bắt buộc IdHocVien phải là chính sinh viên đang đăng nhập
                cd.IdHocVien = idSinhVien;
            }

            // Kiểm tra trùng mã
            if (_context.ChuyenDeNCKHs.Any(x => x.MaSoCD == cd.MaSoCD))
            {
                return BadRequest("Mã chuyên đề đã tồn tại!");
            }

            _context.ChuyenDeNCKHs.Add(cd);
            await _context.SaveChangesAsync();
            return Ok(cd);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRoles.Admin)] // ✅ Chỉ Admin mới được xóa
        public async Task<IActionResult> Delete(int id)
        {
            var cd = await _context.ChuyenDeNCKHs.FindAsync(id);
            if (cd == null) return NotFound();

            // Xóa các bản ghi liên quan (giữ nguyên logic cũ)
            var xepGiais = _context.XepGiais.Where(x => x.IdChuyenDe == id);
            _context.XepGiais.RemoveRange(xepGiais);

            var phieuChams = _context.PhieuChams.Where(x => x.IdChuyenDe == id);
            _context.PhieuChams.RemoveRange(phieuChams);

            var soLoais = _context.KetQuaSoLoais.Where(x => x.IdChuyenDe == id);
            _context.KetQuaSoLoais.RemoveRange(soLoais);

            var nops = _context.NopSanPhams.Where(x => x.IdChuyenDe == id);
            _context.NopSanPhams.RemoveRange(nops);

            var hoiDongs = _context.HoiDongs.Include(h => h.ThanhViens).Where(x => x.IdChuyenDe == id);
            foreach (var hd in hoiDongs)
            {
                _context.ThanhVienHoiDongs.RemoveRange(hd.ThanhViens);
            }
            _context.HoiDongs.RemoveRange(hoiDongs);

            _context.ChuyenDeNCKHs.Remove(cd);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}