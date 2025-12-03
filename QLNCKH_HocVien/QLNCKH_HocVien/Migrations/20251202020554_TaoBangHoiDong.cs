using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNCKH_HocVien.Migrations
{
    /// <inheritdoc />
    public partial class TaoBangHoiDong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HoiDongs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdChuyenDe = table.Column<int>(type: "int", nullable: false),
                    VongThi = table.Column<int>(type: "int", nullable: false),
                    NgayCham = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiaDiem = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoiDongs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThanhVienHoiDongs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdHoiDong = table.Column<int>(type: "int", nullable: false),
                    IdGiaoVien = table.Column<int>(type: "int", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoiDongId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhVienHoiDongs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThanhVienHoiDongs_HoiDongs_HoiDongId",
                        column: x => x.HoiDongId,
                        principalTable: "HoiDongs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThanhVienHoiDongs_HoiDongId",
                table: "ThanhVienHoiDongs",
                column: "HoiDongId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThanhVienHoiDongs");

            migrationBuilder.DropTable(
                name: "HoiDongs");
        }
    }
}
