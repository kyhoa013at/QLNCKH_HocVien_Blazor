// QLNCKH_HocVien/QLNCKH_HocVien/Controllers/KetQuaController.cs
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
    [Authorize] // ✅ Yêu cầu đăng nhập
    public class KetQuaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KetQuaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- VÒNG SƠ LOẠI ---
        [HttpGet("soloa-all")]
        public async Task<ActionResult<List<KetQuaSoLoai>>> GetSoLoai()
            => await _context.KetQuaSoLoais.ToListAsync();

        // ✅ Chỉ Admin và Giáo viên mới được lưu điểm sơ loại
        [HttpPost("soloa")]
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.GiaoVien}")]
        public async Task<ActionResult> SaveSoLoai(KetQuaSoLoai item)
        {
            var exists = await _context.KetQuaSoLoais.FirstOrDefaultAsync(x => x.IdChuyenDe == item.IdChuyenDe);
            if (exists != null)
            {
                exists.DiemSo = item.DiemSo;
                exists.KetQua = item.KetQua;
                exists.NhanXet = item.NhanXet;
            }
            else
            {
                _context.KetQuaSoLoais.Add(item);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        // ✅ Chỉ Admin tự động xét Top 15
        [HttpPost("auto-top15")]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<ActionResult> AutoTop15()
        {
            await _context.Database.ExecuteSqlRawAsync("UPDATE KetQuaSoLoais SET KetQua = 0");

            var listDiem = await (from kq in _context.KetQuaSoLoais
                                  join cd in _context.ChuyenDeNCKHs on kq.IdChuyenDe equals cd.Id
                                  select new { kq, cd.IdLinhVuc }).ToListAsync();

            var listPass = listDiem.GroupBy(x => x.IdLinhVuc)
                                   .SelectMany(g => g.OrderByDescending(x => x.kq.DiemSo).Take(15))
                                   .Select(x => x.kq)
                                   .ToList();

            foreach (var item in listPass) item.KetQua = true;
            await _context.SaveChangesAsync();
            return Ok("Đã xét duyệt Top 15 thành công!");
        }

        // --- VÒNG CHUNG KHẢO (PHIẾU CHẤM) ---
        [HttpGet("phieucham/{idChuyenDe}")]
        public async Task<ActionResult<List<PhieuCham>>> GetPhieuCham(int idChuyenDe)
            => await _context.PhieuChams.Where(x => x.IdChuyenDe == idChuyenDe).ToListAsync();

        // ✅ Giáo viên chỉ được chấm điểm nếu là thành viên hội đồng
        [HttpPost("phieucham")]
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.GiaoVien}")]
        public async Task<ActionResult> SavePhieuCham(PhieuCham pc)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            // ✅ Nếu là Giáo viên, kiểm tra có phải thành viên hội đồng không
            if (role == AppRoles.GiaoVien)
            {
                var idGiaoVienClaim = User.FindFirst("IdGiaoVien")?.Value;
                if (string.IsNullOrEmpty(idGiaoVienClaim) || !int.TryParse(idGiaoVienClaim, out int idGiaoVien))
                {
                    return BadRequest("Không tìm thấy thông tin giáo viên");
                }

                // Kiểm tra giáo viên có trong hội đồng chấm chuyên đề này không
                var isInHoiDong = await _context.HoiDongs
                    .Include(h => h.ThanhViens)
                    .AnyAsync(h => h.IdChuyenDe == pc.IdChuyenDe &&
                                   h.ThanhViens.Any(tv => tv.IdGiaoVien == idGiaoVien));

                if (!isInHoiDong)
                {
                    return Forbid("Bạn không phải thành viên hội đồng chấm chuyên đề này");
                }

                // Bắt buộc IdGiaoVien phải là chính giáo viên đang đăng nhập
                pc.IdGiaoVien = idGiaoVien;
            }

            var exists = await _context.PhieuChams
                .FirstOrDefaultAsync(x => x.IdChuyenDe == pc.IdChuyenDe && x.IdGiaoVien == pc.IdGiaoVien);

            if (exists != null)
            {
                exists.Diem = pc.Diem;
                exists.YKien = pc.YKien;
            }
            else
            {
                _context.PhieuChams.Add(pc);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("phieucham-all")]
        public async Task<ActionResult<List<PhieuCham>>> GetAllPhieuCham()
        {
            return await _context.PhieuChams.ToListAsync();
        }
    }
}