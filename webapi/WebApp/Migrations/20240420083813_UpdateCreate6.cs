using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCreate6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockInType",
                table: "DrugStockIn");

            migrationBuilder.RenameColumn(
                name: "StockId",
                table: "DrugStockInDetail",
                newName: "StockInNo");

            migrationBuilder.RenameColumn(
                name: "StockId",
                table: "DrugStockIn",
                newName: "StockInNo");

            migrationBuilder.AlterColumn<int>(
                name: "StockOutId",
                table: "DrugStockOut",
                type: "INTEGER",
                nullable: false,
                comment: "出库主键",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "入库编号")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "OutWay",
                table: "DrugStockOut",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                comment: "出库方式");

            migrationBuilder.AddColumn<int>(
                name: "StockOutNo",
                table: "DrugStockOut",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                comment: "出库编号");

            migrationBuilder.AddColumn<string>(
                name: "DrugName",
                table: "DrugStockInDetail",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                comment: "药品名称");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "DrugStockInDetail",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "有效期");

            migrationBuilder.AddColumn<int>(
                name: "InWay",
                table: "DrugStockIn",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                comment: "入库方式");

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "DrugStockIn",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                comment: "制造商--这个只需要名字就行了");

            migrationBuilder.AlterColumn<int>(
                name: "EventType",
                table: "DrugSendBatchOrder",
                type: "INTEGER",
                nullable: false,
                comment: "入库,出库,调拨，增益，报损",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "出库或调拨");

            migrationBuilder.CreateTable(
                name: "DrugInOutInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReceiptNo = table.Column<string>(type: "TEXT", nullable: false, comment: "单据号"),
                    EventType = table.Column<int>(type: "INTEGER", nullable: false, comment: "0(入库,增益),1(出库,报损),2(调拨)"),
                    InOutWay = table.Column<int>(type: "INTEGER", nullable: false, comment: "进出库方式，入库方式具体看入库表，出库方式看出库表"),
                    DrugId = table.Column<int>(type: "INTEGER", nullable: false),
                    InOutQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    OriginalQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugInOutInfo", x => x.Id);
                },
                comment: "入库出库调拨记录");

            migrationBuilder.CreateTable(
                name: "DrugWarehouse",
                columns: table => new
                {
                    StockId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StockNo = table.Column<string>(type: "TEXT", nullable: false),
                    DrugId = table.Column<int>(type: "INTEGER", nullable: false),
                    DrugName = table.Column<string>(type: "TEXT", nullable: false),
                    WarehouseNo = table.Column<int>(type: "INTEGER", nullable: false),
                    WarehouseName = table.Column<string>(type: "TEXT", nullable: false),
                    ActQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    NeedSendQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugWarehouse", x => x.StockId);
                });

            migrationBuilder.CreateTable(
                name: "DrugWarehouseDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StockNo = table.Column<string>(type: "TEXT", nullable: false),
                    DrugId = table.Column<int>(type: "INTEGER", nullable: false),
                    WarehoseNo = table.Column<int>(type: "INTEGER", nullable: false),
                    BatchNo = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugWarehouseDetail", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugInOutInfo");

            migrationBuilder.DropTable(
                name: "DrugWarehouse");

            migrationBuilder.DropTable(
                name: "DrugWarehouseDetail");

            migrationBuilder.DropColumn(
                name: "OutWay",
                table: "DrugStockOut");

            migrationBuilder.DropColumn(
                name: "StockOutNo",
                table: "DrugStockOut");

            migrationBuilder.DropColumn(
                name: "DrugName",
                table: "DrugStockInDetail");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "DrugStockInDetail");

            migrationBuilder.DropColumn(
                name: "InWay",
                table: "DrugStockIn");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "DrugStockIn");

            migrationBuilder.RenameColumn(
                name: "StockInNo",
                table: "DrugStockInDetail",
                newName: "StockId");

            migrationBuilder.RenameColumn(
                name: "StockInNo",
                table: "DrugStockIn",
                newName: "StockId");

            migrationBuilder.AlterColumn<int>(
                name: "StockOutId",
                table: "DrugStockOut",
                type: "INTEGER",
                nullable: false,
                comment: "入库编号",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "出库主键")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "StockInType",
                table: "DrugStockIn",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                comment: "入库类别");

            migrationBuilder.AlterColumn<int>(
                name: "EventType",
                table: "DrugSendBatchOrder",
                type: "INTEGER",
                nullable: false,
                comment: "出库或调拨",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "入库,出库,调拨，增益，报损");
        }
    }
}
