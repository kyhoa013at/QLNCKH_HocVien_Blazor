using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNCKH_HocVien.Client.Models;
using QLNCKH_HocVien.Data;

namespace QLNCKH_HocVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SinhVienController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SinhVienController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SinhVien (Lấy danh sách)
        [HttpGet]
        public async Task<ActionResult<List<SinhVien>>> GetSinhViens()
        {
            return await _context.SinhViens.ToListAsync();
        }

        // POST: api/SinhVien (Thêm mới)
        [HttpPost]
        public async Task<ActionResult<SinhVien>> CreateSinhVien(SinhVien sv)
        {
            _context.SinhViens.Add(sv);
            await _context.SaveChangesAsync();
            return Ok(sv);
        }

        // DELETE: api/SinhVien/5 (Xóa theo ID)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSinhVien(int id)
        {
            var sv = await _context.SinhViens.FindAsync(id);
            if (sv == null) return NotFound();

            _context.SinhViens.Remove(sv);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //EDIT Thông tin Sinh viên
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSinhVien(int id, SinhVien sv)
        {
            if (id != sv.Id) return BadRequest();

            // Tìm sinh viên cũ
            var existingSv = await _context.SinhViens.FindAsync(id);
            if (existingSv == null) return NotFound();

            // Cập nhật thông tin (EF Core tự theo dõi thay đổi hoặc gán đè)
            // Cách đơn giản nhất là gán giá trị mới sang
            _context.Entry(existingSv).CurrentValues.SetValues(sv);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}