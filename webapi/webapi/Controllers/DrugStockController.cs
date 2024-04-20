using Microsoft.AspNetCore.Mvc;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DrugStockController(MyContext drugContext, IdGeneratorService idGenerator) : ControllerBase
    {
        public static void StockIn(MyContext drugContext, DrugStockIn stockIn)
        {
            var receiptNo = stockIn.ReceiptNo;
            if (string.IsNullOrWhiteSpace(stockIn.ReceiptNo))
            {
                throw new Exception("单据号不能为空");
            }
            var warehouseNo = stockIn.WarehoueNo;

            var stockInDetail = drugContext.DrugStockInDetail.Where(t => t.ReceiptNo == stockIn.ReceiptNo && t.StockInNo == stockIn.StockInNo).Select(t => new StockInDetail()
            {
                BatchNo = t.BatchNo,
                DrugId = t.DrugId,
                Quantity = t.InQuantity,
            }).ToList();

            if (stockInDetail.Count() == 0)
            {
                throw new Exception("该单据下没有数据要审核");
            }

            var drugIds = stockInDetail.Select(t => t.DrugId);
            var warehouse = drugContext.DrugWarehouse.Where(t => drugIds.Contains(t.DrugId) && t.WarehouseNo == warehouseNo)
                .ToDictionary(t => t.DrugId, t => t);
            var drugInfos = drugContext.Drugs.Where(t => drugIds.Contains(t.DrugId))
                .ToDictionary(t => t.DrugId, t => t.DrugName);
            var guid = Guid.NewGuid().ToString();
            DateTime dt = DateTime.Now;
            List<DrugWarehouseDetail> drugDetail = new List<DrugWarehouseDetail>();
            List<DrugWarehouse> drugwarehouse = new List<DrugWarehouse>();
            List<DrugInOutInfo> inoutInfo = new List<DrugInOutInfo>();

            foreach (var item in stockInDetail)
            {
                var detail = new DrugWarehouseDetail()
                {
                    Id = 0,
                    StockNo = guid,
                    DrugId = item.DrugId,
                    WarehoseNo = warehouseNo,
                    BatchNo = item.BatchNo,
                    Quantity = item.Quantity,
                    CreateTime = dt,
                    ExpirationDate = item.ExpirationDate,
                };
                drugDetail.Add(detail);
            }

            foreach (var item in stockInDetail)
            {
                DrugInOutInfo drugStockInfo = new DrugInOutInfo();
                drugStockInfo.DrugId = item.DrugId;
                drugStockInfo.ReceiptNo = receiptNo;
                drugStockInfo.EventType = 0;
                drugStockInfo.CreateTime = dt;
                drugStockInfo.InOutWay = stockIn.InWay;

                if (warehouse.ContainsKey(item.DrugId))
                {
                    var drugWareHose = warehouse[item.DrugId];
                    drugWareHose.ActQuantity = drugWareHose.ActQuantity + item.Quantity;

                    drugStockInfo.OriginalQuantity = drugWareHose.ActQuantity - item.Quantity;
                    drugStockInfo.CurrentQuantity = drugWareHose.ActQuantity;
                    drugStockInfo.InOutQuantity = item.Quantity;
                }
                else
                {
                    var newDrugMain = new DrugWarehouse()
                    {
                        StockId = 0,
                        StockNo = guid,
                        DrugId = item.DrugId,
                        DrugName = drugInfos[item.DrugId],
                        WarehouseNo = warehouseNo,
                        WarehouseName = "",
                        ActQuantity = item.Quantity,
                        NeedSendQuantity = 0,
                        CreateTime = dt,
                    };
                    drugwarehouse.Add(newDrugMain);

                    drugStockInfo.OriginalQuantity = 0;
                    drugStockInfo.CurrentQuantity = item.Quantity;
                    drugStockInfo.InOutQuantity = item.Quantity;
                }

                inoutInfo.Add(drugStockInfo);
            }

            drugContext.DrugWarehouse.AddRange(drugwarehouse);
            drugContext.DrugWarehouseDetail.AddRange(drugDetail);
            drugContext.DrugInOutInfo.AddRange(inoutInfo);
        }
    }
    public class StockInDetail()
    {
        public int DrugId { get; set; }
        public int Quantity { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class StockOutDetail()
    {
        public int DrugId { get; set; }
        public int Quantity { get; set; }
        public string BatchNo { get; set; }
    }
}
