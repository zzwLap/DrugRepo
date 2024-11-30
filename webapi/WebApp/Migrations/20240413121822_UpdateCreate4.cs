using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCreate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drugs",
                columns: table => new
                {
                    DrugId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DrugName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "药品名称"),
                    Spec = table.Column<string>(type: "TEXT", nullable: false, comment: "药品规格"),
                    PinYin = table.Column<string>(type: "TEXT", nullable: false, comment: "拼音码"),
                    Sort = table.Column<int>(type: "INTEGER", nullable: false, comment: "排序编号"),
                    ClinicalUnit = table.Column<string>(type: "TEXT", nullable: false, comment: "临床单位"),
                    PackageUnit = table.Column<string>(type: "TEXT", nullable: false, comment: "包装单位"),
                    C2PQuantity = table.Column<int>(type: "INTEGER", nullable: false, comment: "临床到包装单位的转换"),
                    ApprovalNumber = table.Column<string>(type: "TEXT", nullable: false, comment: "批准文号"),
                    DrugCode = table.Column<string>(type: "TEXT", nullable: false, comment: "本地药品编码"),
                    NationDrugCode = table.Column<string>(type: "TEXT", nullable: false, comment: "国家药品编码"),
                    RADManufacturer = table.Column<string>(type: "TEXT", nullable: false, comment: "药品研发厂家"),
                    DosageForm = table.Column<string>(type: "TEXT", nullable: false, comment: "药品剂型"),
                    DefaultSaleUnit = table.Column<int>(type: "INTEGER", nullable: false, comment: "销售单位"),
                    ClinicalSaleUnit = table.Column<int>(type: "INTEGER", nullable: false, comment: "临床销售单位"),
                    PackagePrice = table.Column<decimal>(type: "TEXT", precision: 16, scale: 4, nullable: false, comment: "包装单位销售单价"),
                    ClinicalPrice = table.Column<decimal>(type: "TEXT", precision: 16, scale: 4, nullable: false, comment: "临床销售单价")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drugs", x => x.DrugId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Drugs");
        }
    }
}
