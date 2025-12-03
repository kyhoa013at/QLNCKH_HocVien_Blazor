using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNCKH_HocVien.Migrations
{
    /// <inheritdoc />
    public partial class TaoBangXepGiai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "XepGiais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdChuyenDe = table.Column<int>(type: "int", nullable: false),
                    DiemTrungBinh = table.Column<double>(type: "float", nullable: false),
                    TenGiai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    XepHang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XepGiais", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "XepGiais");
        }
    }
}
