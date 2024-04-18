using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DrugStockController(MyContext drugContext, IdGeneratorService idGenerator) : ControllerBase
    {
        [HttpGet]
        public Task<List<DisplayDrug>> GetDrugs(QueryDrugDto query)
        {
            var drug = drugContext.Drugs.AsQueryable();

            if (String.IsNullOrWhiteSpace(query.PinYin))
            {
                drug = drug.Where(t => t.PinYin.StartsWith(query.PinYin));
            }
            if (String.IsNullOrWhiteSpace(query.DrugName))
            {
                drug = drug.Where(t => t.DrugName.StartsWith(query.DrugName));
            }

            return drug.OrderBy(t => t.Sort).SelectByType<Drugs, DisplayDrug>().ToListAsync();
        }

        [HttpPost]
        public async Task AddDrug(Tuple<AddStockIn, List<AddStockInDetail>> stockInfo)
        {
            var stockId = idGenerator.GetNextId();
            var receiptNo = idGenerator.GenerateNextReceiptId("");

            var dt = DateTime.Now;
            var stockMain = stockInfo.Item1;
            DrugStockIn stockIn = new DrugStockIn();
            stockIn.Creator = "admin";
            stockIn.CreatorId = this.GetUserId();
            stockIn.WarehoueNo = stockMain.WarehouseNo;
            stockIn.WarehoueName = stockMain.WarehouseName;
            stockIn.Remarks = stockMain.Remarks;
            stockIn.ReceiptNo = receiptNo;

            List<DrugStockInDetail> stockDetail = new List<DrugStockInDetail>();
            foreach (var item in stockInfo.Item2)
            {
                DrugStockInDetail detail = new DrugStockInDetail();
                detail.DrugId = item.DrugId;
                detail.StockId = stockId;
                detail.ReceiptNo = receiptNo;
                detail.BatchNo = item.BatchNo;
                //TODO 需要修改
                detail.InQuantity = item.InputQuantity * 1;
                detail.InputQuantity = item.InputQuantity;
                detail.PurchasePrice = item.PurchasePrice;
                detail.CreateTime = dt;
                detail.UnitType = item.UnitType;
                stockDetail.Add(detail);
            }
            drugContext.DrugStockIn.Add(stockIn);
            drugContext.DrugStockInDetail.AddRange(stockDetail);
            await drugContext.SaveChangesAsync();
        }

        [HttpPut("{recepitNo}")]
        [HttpPatch("{recepitNo}")]
        public async Task ModifyDrugs(string recepitNo, List<AddStockInDetail> adds)
        {
            var stockIn = drugContext.DrugStockIn.SingleOrDefault(t => t.ReceiptNo == recepitNo);
            if (stockIn == null)
            {
                throw new Exception("没有找到该药品信息");
            }
            var drugs = adds.Select(t => t.DrugId);

            var list = drugContext.DrugStockInDetail.Where(t => t.ReceiptNo == stockIn.ReceiptNo && t.StockId == stockIn.StockId
                             && drugs.Contains(t.DrugId)).ToDictionary(t => t.DrugId, t => t);

            DateTime dt = DateTime.Now;
            foreach (var item in adds)
            {
                if (list.ContainsKey(item.DrugId))
                {
                    var v = list[item.DrugId];
                    v.BatchNo = item.BatchNo;
                    v.PurchasePrice = item.PurchasePrice;
                    v.InQuantity = item.InputQuantity * 1;
                    v.InputQuantity = item.InputQuantity;
                    v.UnitType = item.UnitType;
                }
                else
                {
                    drugContext.DrugStockInDetail.Add(new DrugStockInDetail()
                    {
                        DrugId = item.DrugId,
                        StockId = stockIn.StockId,
                        BatchNo = item.BatchNo,
                        CreateTime = dt,
                        InputQuantity = item.InputQuantity,
                        InQuantity = item.InputQuantity,
                        PurchasePrice = item.PurchasePrice,
                        ReceiptNo = stockIn.ReceiptNo,
                        UnitType = item.UnitType,
                        Id = 0,
                    });
                }
            }

            await drugContext.SaveChangesAsync();
        }

        [HttpPut("{recepitNo}")]
        public async Task ApproveStockRecord(string recepitNo)
        {
            var stockIn = drugContext.DrugStockIn.SingleOrDefault(t => t.ReceiptNo == recepitNo);
            if (stockIn == null)
            {
                throw new Exception("未找到单据信息");
            }
            if (stockIn.Status == 1)
            {
                throw new Exception("只有单据在提交状态时才能审核");
            }
            stockIn.Status = 1;
            await drugContext.SaveChangesAsync();
        }

        [HttpDelete("{recepitNo}")]
        public async Task DeleteStockRecord(string recepitNo)
        {
            var stockIn = drugContext.DrugStockIn.SingleOrDefault(t => t.ReceiptNo == recepitNo);
            if (stockIn == null)
            {
                throw new Exception("未找到单据信息");
            }
            if (stockIn.Status == 2)
            {
                throw new Exception("已经审核的单据不允许删除");
            }
            if (stockIn.Status == 0)
            {
                throw new Exception("只有单据在草稿状态时才能被删除，请撤销提交后再删除");
            }
            await drugContext.DrugStockIn.Where(t => t.ReceiptNo == recepitNo).ExecuteDeleteAsync();
            await drugContext.DrugStockInDetail.Where(t => t.ReceiptNo == recepitNo).ExecuteDeleteAsync();
        }
    }

    public class QueryDrugDto
    {
        public string PinYin { get; set; }
        public string DrugName { get; set; }
    }

    public class DisplayDrug
    {
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public string Spec { get; set; }
        public string PinYin { get; set; }
        public string ClinicalUnit { get; set; }
        public string PackageUnit { get; set; }
        public int C2PQuantity { get; set; }
        public string ApprovalNumber { get; set; }
        public string DrugCode { get; set; }
        public string DosageForm { get; set; }
        public decimal PackagePrice { get; set; }
    }

    public class AddStockIn
    {
        public int WarehouseNo { get; set; }
        public string WarehouseName { get; set; }
        public string Remarks { get; set; }
    }

    public class AddStockInDetail
    {
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public string BatchNo { get; set; }
        public int InputQuantity { get; set; }
        public int UnitType { get; set; }
        public decimal PurchasePrice { get; set; }
    }

    public class IdGeneratorService
    {
        public string GetNextId()
        {
            return NewId.Next().ToString();
        }
        public string GenerateNextReceiptId(string currentReceiptId)
        {
            return Guid.NewGuid().ToString();
        }
    }
}
