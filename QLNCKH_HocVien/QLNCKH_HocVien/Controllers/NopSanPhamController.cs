using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNCKH_HocVien.Client.Models;
using QLNCKH_HocVien.Data;

namespace QLNCKH_HocVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NopSanPhamController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NopSanPhamController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<NopSanPham>>> GetAll()
        {
            return await _context.NopSanPhams.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<NopSanPham>> Create(NopSanPham item)
        {
            // Logic kiểm tra xem đã nộp trạng thái này chưa (nếu cần)
            _context.NopSanPhams.Add(item);
            await _context.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.NopSanPhams.FindAsync(id);
            if (item == null) return NotFound();
            _context.NopSanPhams.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}