using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCreate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockInId",
                table: "DrugStockInDetail");

            migrationBuilder.DropColumn(
                name: "BatchNo",
                table: "DrugStockIn");

            migrationBuilder.AddColumn<string>(
                name: "BatchNo",
                table: "DrugStockInDetail",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                comment: "批次号");

            migrationBuilder.AddColumn<int>(
                name: "InputQuantity",
                table: "DrugStockInDetail",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                comment: "输入数量");

            migrationBuilder.AddColumn<string>(
                name: "StockId",
                table: "DrugStockInDetail",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                comment: "入库编号");

            migrationBuilder.AddColumn<int>(
                name: "UnitType",
                table: "DrugStockInDetail",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                comment: "入库单位类别，0包装单位，1零售单位");

            migrationBuilder.AlterColumn<int>(
                name: "WarehoueNo",
                table: "DrugStockIn",
                type: "INTEGER",
                nullable: false,
                comment: "库房编号",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldComment: "库房编号");

            migrationBuilder.AlterColumn<int>(
                name: "StockInId",
                table: "DrugStockIn",
                type: "INTEGER",
                nullable: false,
                comment: "入库自增长id编号",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "入库编号")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "DrugStockIn",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                comment: "0草稿，1提交，审核");

            migrationBuilder.AddColumn<string>(
                name: "StockId",
                table: "DrugStockIn",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                comment: "入库编号-生成的编号");

            migrationBuilder.AlterColumn<string>(
                name: "RADManufacturer",
                table: "Drugs",
                type: "TEXT",
                nullable: true,
                comment: "药品研发厂家",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldComment: "药品研发厂家");

            migrationBuilder.AlterColumn<string>(
                name: "PinYin",
                table: "Drugs",
                type: "TEXT",
                nullable: true,
                comment: "拼音码",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldComment: "拼音码");

            migrationBuilder.AlterColumn<string>(
                name: "NationDrugCode",
                table: "Drugs",
                type: "TEXT",
                nullable: true,
                comment: "国家药品编码",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldComment: "国家药品编码");

            migrationBuilder.AlterColumn<string>(
                name: "DrugCode",
                table: "Drugs",
                type: "TEXT",
                nullable: true,
                comment: "本地药品编码",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldComment: "本地药品编码");

            migrationBuilder.AlterColumn<string>(
                name: "DosageForm",
                table: "Drugs",
                type: "TEXT",
                nullable: true,
                comment: "药品剂型",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldComment: "药品剂型");

            migrationBuilder.AlterColumn<string>(
                name: "ApprovalNumber",
                table: "Drugs",
                type: "TEXT",
                nullable: true,
                comment: "批准文号",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldComment: "批准文号");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchNo",
                table: "DrugStockInDetail");

            migrationBuilder.DropColumn(
                name: "InputQuantity",
                table: "DrugStockInDetail");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "DrugStockInDetail");

            migrationBuilder.DropColumn(
                name: "UnitType",
                table: "DrugStockInDetail");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DrugStockIn");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "DrugStockIn");

            migrationBuilder.AddColumn<int>(
                name: "StockInId",
                table: "DrugStockInDetail",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                comment: "单据号");

            migrationBuilder.AlterColumn<string>(
                name: "WarehoueNo",
                table: "DrugStockIn",
                type: "TEXT",
                nullable: false,
                comment: "库房编号",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "库房编号");

            migrationBuilder.AlterColumn<int>(
                name: "StockInId",
                table: "DrugStockIn",
                type: "INTEGER",
                nullable: false,
                comment: "入库编号",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "入库自增长id编号")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "BatchNo",
                table: "DrugStockIn",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                comment: "批次号");

            migrationBuilder.AlterColumn<string>(
                name: "RADManufacturer",
                table: "Drugs",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                comment: "药品研发厂家",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "药品研发厂家");

            migrationBuilder.AlterColumn<string>(
                name: "PinYin",
                table: "Drugs",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                comment: "拼音码",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "拼音码");

            migrationBuilder.AlterColumn<string>(
                name: "NationDrugCode",
                table: "Drugs",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                comment: "国家药品编码",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "国家药品编码");

            migrationBuilder.AlterColumn<string>(
                name: "DrugCode",
                table: "Drugs",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                comment: "本地药品编码",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "本地药品编码");

            migrationBuilder.AlterColumn<string>(
                name: "DosageForm",
                table: "Drugs",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                comment: "药品剂型",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "药品剂型");

            migrationBuilder.AlterColumn<string>(
                name: "ApprovalNumber",
                table: "Drugs",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                comment: "批准文号",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "批准文号");
        }
    }
}
