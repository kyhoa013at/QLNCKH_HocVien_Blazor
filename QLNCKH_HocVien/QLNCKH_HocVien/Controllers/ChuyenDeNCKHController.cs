using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNCKH_HocVien.Client.Models;
using QLNCKH_HocVien.Data;

namespace QLNCKH_HocVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            return await _context.ChuyenDeNCKHs.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ChuyenDeNCKH>> Create(ChuyenDeNCKH cd)
        {
            // Kiểm tra trùng mã nếu cần
            if (_context.ChuyenDeNCKHs.Any(x => x.MaSoCD == cd.MaSoCD))
            {
                return BadRequest("Mã chuyên đề đã tồn tại!");
            }

            _context.ChuyenDeNCKHs.Add(cd);
            await _context.SaveChangesAsync();
            return Ok(cd);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cd = await _context.ChuyenDeNCKHs.FindAsync(id);
            if (cd == null) return NotFound();

            // 1. Xóa Xếp giải cũ liên quan
            var xepGiais = _context.XepGiais.Where(x => x.IdChuyenDe == id);
            _context.XepGiais.RemoveRange(xepGiais);

            // 2. Xóa Phiếu chấm liên quan
            var phieuChams = _context.PhieuChams.Where(x => x.IdChuyenDe == id);
            _context.PhieuChams.RemoveRange(phieuChams);

            // 3. Xóa Kết quả sơ loại
            var soLoais = _context.KetQuaSoLoais.Where(x => x.IdChuyenDe == id);
            _context.KetQuaSoLoais.RemoveRange(soLoais);

            // 4. Xóa Nộp sản phẩm
            var nops = _context.NopSanPhams.Where(x => x.IdChuyenDe == id);
            _context.NopSanPhams.RemoveRange(nops);

            // 5. Xóa Hội đồng (Cần xóa thành viên trước nếu chưa cấu hình Cascade trong DB)
            var hoiDongs = _context.HoiDongs.Include(h => h.ThanhViens).Where(x => x.IdChuyenDe == id);
            foreach (var hd in hoiDongs)
            {
                _context.ThanhVienHoiDongs.RemoveRange(hd.ThanhViens);
            }
            _context.HoiDongs.RemoveRange(hoiDongs);

            // 6. Cuối cùng mới xóa Chuyên đề
            _context.ChuyenDeNCKHs.Remove(cd);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}