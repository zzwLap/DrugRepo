using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataDictionary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<int>(type: "INTEGER", nullable: false, comment: "值"),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, comment: "名称"),
                    Description = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true, comment: "描述"),
                    CategoryName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, comment: "类别名称"),
                    CategoryCode = table.Column<int>(type: "INTEGER", nullable: false, comment: "类别编码--有没有必须呢")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataDictionary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrugSendBatchOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    DrugId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceiptNo = table.Column<string>(type: "TEXT", nullable: false),
                    BatchNo = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    EventType = table.Column<int>(type: "INTEGER", nullable: false, comment: "出库或调拨"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugSendBatchOrder", x => x.Id);
                },
                comment: "执行批次顺序表");

            migrationBuilder.CreateTable(
                name: "DrugStockIn",
                columns: table => new
                {
                    StockInId = table.Column<int>(type: "INTEGER", nullable: false, comment: "入库编号")
                        .Annotation("Sqlite:Autoincrement", true),
                    ReceiptNo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "单据号"),
                    Checker = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "验收人"),
                    CheckerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "创建人"),
                    CreatorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Approver = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "审核人"),
                    ApproverId = table.Column<int>(type: "INTEGER", nullable: false),
                    Remarks = table.Column<string>(type: "TEXT", nullable: false, comment: "备注"),
                    StockInType = table.Column<string>(type: "TEXT", nullable: false, comment: "入库类别"),
                    WarehoueNo = table.Column<string>(type: "TEXT", nullable: false, comment: "库房编号"),
                    WarehoueName = table.Column<string>(type: "TEXT", nullable: false, comment: "库房名称"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BatchNo = table.Column<string>(type: "TEXT", nullable: false, comment: "批次号")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugStockIn", x => x.StockInId);
                },
                comment: "药品入库");

            migrationBuilder.CreateTable(
                name: "DrugStockInDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StockInId = table.Column<int>(type: "INTEGER", nullable: false, comment: "单据号"),
                    ReceiptNo = table.Column<string>(type: "TEXT", nullable: false, comment: "单据号"),
                    DrugId = table.Column<int>(type: "INTEGER", nullable: false, comment: "药品编号"),
                    InQuantity = table.Column<int>(type: "INTEGER", nullable: false, comment: "入库数量"),
                    PurchasePrice = table.Column<decimal>(type: "TEXT", nullable: false, comment: "采购单价"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugStockInDetail", x => x.Id);
                },
                comment: "药品入库明细");

            migrationBuilder.CreateTable(
                name: "DrugStockOut",
                columns: table => new
                {
                    StockOutId = table.Column<int>(type: "INTEGER", nullable: false, comment: "入库编号")
                        .Annotation("Sqlite:Autoincrement", true),
                    ReceiptNo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "单据号"),
                    Checker = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "验收人"),
                    CheckerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "创建人"),
                    CreatorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Approver = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "审核人"),
                    ApproverId = table.Column<int>(type: "INTEGER", nullable: false),
                    Remarks = table.Column<string>(type: "TEXT", nullable: false, comment: "备注"),
                    StockOutType = table.Column<string>(type: "TEXT", nullable: false, comment: "出库类别"),
                    WarehoueNo = table.Column<string>(type: "TEXT", nullable: false, comment: "库房编号"),
                    WarehoueName = table.Column<string>(type: "TEXT", nullable: false, comment: "库房名称"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugStockOut", x => x.StockOutId);
                },
                comment: "药品出库");

            migrationBuilder.CreateTable(
                name: "DrugStockOutDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StockOutId = table.Column<int>(type: "INTEGER", nullable: false, comment: "单据号"),
                    ReceiptNo = table.Column<string>(type: "TEXT", nullable: false, comment: "单据号"),
                    DrugId = table.Column<int>(type: "INTEGER", nullable: false, comment: "药品编号"),
                    InQuantity = table.Column<int>(type: "INTEGER", nullable: false, comment: "入库数量"),
                    StockOutAmount = table.Column<decimal>(type: "TEXT", nullable: false, comment: "出库金额"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugStockOutDetail", x => x.Id);
                },
                comment: "药品出库明细");

            migrationBuilder.CreateTable(
                name: "DrugTransfer",
                columns: table => new
                {
                    TransferId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReceiptNo = table.Column<string>(type: "TEXT", nullable: false, comment: "单据号"),
                    InWarehourseNo = table.Column<int>(type: "INTEGER", nullable: false),
                    InWarehourseName = table.Column<string>(type: "TEXT", nullable: false),
                    OutWarehouseNo = table.Column<int>(type: "INTEGER", nullable: false),
                    OutWarehouseName = table.Column<string>(type: "TEXT", nullable: false),
                    Checker = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "验收人"),
                    CheckerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "创建人"),
                    CreatorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Approver = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "审核人"),
                    ApproverId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugTransfer", x => x.TransferId);
                },
                comment: "药品调拨");

            migrationBuilder.CreateTable(
                name: "DrugTransferDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransferId = table.Column<int>(type: "INTEGER", nullable: false, comment: "单据号"),
                    ReceiptNo = table.Column<string>(type: "TEXT", nullable: false, comment: "单据号"),
                    DrugId = table.Column<int>(type: "INTEGER", nullable: false, comment: "药品编号"),
                    TransferQuantity = table.Column<int>(type: "INTEGER", nullable: false, comment: "入库数量"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugTransferDetail", x => x.Id);
                },
                comment: "药品调拨明细");

            migrationBuilder.CreateTable(
                name: "HierachyDictionary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false, comment: "值"),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, comment: "名称"),
                    Description = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true, comment: "备注"),
                    CategoryName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, comment: "类别名称")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HierachyDictionary", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataDictionary");

            migrationBuilder.DropTable(
                name: "DrugSendBatchOrder");

            migrationBuilder.DropTable(
                name: "DrugStockIn");

            migrationBuilder.DropTable(
                name: "DrugStockInDetail");

            migrationBuilder.DropTable(
                name: "DrugStockOut");

            migrationBuilder.DropTable(
                name: "DrugStockOutDetail");

            migrationBuilder.DropTable(
                name: "DrugTransfer");

            migrationBuilder.DropTable(
                name: "DrugTransferDetail");

            migrationBuilder.DropTable(
                name: "HierachyDictionary");
        }
    }
}
