using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNCKH_HocVien.Client.Models;
using QLNCKH_HocVien.Data;

namespace QLNCKH_HocVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpPost("soloa")]
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

        // API Tự động chọn Top 15 theo điểm cao nhất mỗi lĩnh vực
        [HttpPost("auto-top15")]
        public async Task<ActionResult> AutoTop15()
        {
            // 1. Reset toàn bộ về False
            await _context.Database.ExecuteSqlRawAsync("UPDATE KetQuaSoLoais SET KetQua = 0");

            // 2. Lấy danh sách điểm kèm thông tin Lĩnh vực
            var listDiem = await (from kq in _context.KetQuaSoLoais
                                  join cd in _context.ChuyenDeNCKHs on kq.IdChuyenDe equals cd.Id
                                  select new { kq, cd.IdLinhVuc }).ToListAsync();

            // 3. Group theo lĩnh vực và lấy Top 15
            var listPass = listDiem.GroupBy(x => x.IdLinhVuc)
                                   .SelectMany(g => g.OrderByDescending(x => x.kq.DiemSo).Take(15))
                                   .Select(x => x.kq)
                                   .ToList();

            // 4. Update trạng thái
            foreach (var item in listPass) item.KetQua = true;
            await _context.SaveChangesAsync();
            return Ok("Đã xét duyệt Top 15 thành công!");
        }

        // --- VÒNG CHUNG KHẢO (PHIẾU CHẤM) ---
        [HttpGet("phieucham/{idChuyenDe}")]
        public async Task<ActionResult<List<PhieuCham>>> GetPhieuCham(int idChuyenDe)
            => await _context.PhieuChams.Where(x => x.IdChuyenDe == idChuyenDe).ToListAsync();

        [HttpPost("phieucham")]
        public async Task<ActionResult> SavePhieuCham(PhieuCham pc)
        {
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
    }
}