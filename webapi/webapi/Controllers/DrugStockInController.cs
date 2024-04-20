using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Dto;
using webapi.Models;
using webapi.Vo;

namespace webapi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DrugStockInController(MyContext drugContext, IdGeneratorService idGenerator, CacheInfo cache) : ControllerBase
    {
        /// <summary>
        /// 获取药品信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<List<DisplayDrugDto>> GetDrugs(QueryDrugDto query)
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

            return drug.OrderBy(t => t.Sort).SelectByType<Drugs, DisplayDrugDto>().ToListAsync();
        }
        /// <summary>
        /// 获取库房信息
        /// </summary>
        /// <param name="stockInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddDrug(Tuple<AddStockInVo, List<AddStockInDetailVo>> stockInfo)
        {
            ValidateStockData(stockInfo.Item2);

            var stockId = idGenerator.GetNextId();
            var receiptNo = idGenerator.GenerateNextReceiptId("");
            var user = await cache.GetUserAsync(this.GetUserId());
            var dt = DateTime.Now;
            var stockMain = stockInfo.Item1;
            DrugStockIn stockIn = new DrugStockIn();
            stockIn.Creator = user.UserName;
            stockIn.CreatorId = user.UserId;
            stockIn.WarehoueNo = stockMain.WarehouseNo;
            stockIn.WarehoueName = stockMain.WarehouseName;
            stockIn.Remarks = stockMain.Remarks;
            stockIn.ReceiptNo = receiptNo;
            stockIn.InWay = stockMain.InWay;

            List<DrugStockInDetail> stockDetail = new List<DrugStockInDetail>();
            foreach (var item in stockInfo.Item2)
            {
                DrugStockInDetail detail = new DrugStockInDetail();
                detail.DrugId = item.DrugId;
                detail.StockInNo = stockId;
                detail.ReceiptNo = receiptNo;
                detail.BatchNo = item.BatchNo;
                //TODO 需要修改
                detail.InputQuantity = item.InputQuantity;
                detail.PurchasePrice = item.PurchasePrice;
                detail.CreateTime = dt;
                detail.UnitType = item.UnitType;

                detail.DrugName = "";
                detail.InQuantity = item.InputQuantity * 1;
                stockDetail.Add(detail);
            }
            drugContext.DrugStockIn.Add(stockIn);
            drugContext.DrugStockInDetail.AddRange(stockDetail);
            await drugContext.SaveChangesAsync();
        }
        /// <summary>
        /// 修改入库药品信息
        /// </summary>
        /// <param name="recepitNo"></param>
        /// <param name="adds"></param>
        /// <returns></returns>
        [HttpPut("{recepitNo}")]
        [HttpPatch("{recepitNo}")]
        public async Task ModifyDrugs(string recepitNo, List<AddStockInDetailVo> adds)
        {
            ValidateStockData(adds);
            await RecreateAllDrugs(recepitNo, adds);
        }

        private void ValidateStockData(List<AddStockInDetailVo> adds)
        {
            var check = adds.ToLookup(t => t.DrugId);

            var d2 = check.Where(t => t.Count() > 1);

            if (d2.Any())
            {
                throw new Exception("一次入库不允许重复出现多种相同类别的药");
            }
        }
        private async Task RecreateAllDrugs(string recepitNo, List<AddStockInDetailVo> adds)
        {
            var stockIn = drugContext.DrugStockIn.SingleOrDefault(t => t.ReceiptNo == recepitNo);
            if (stockIn == null)
            {
                throw new Exception("没有找到该单据信息");
            }

            stockIn.ReceiptNo = recepitNo;
            DateTime dt = DateTime.Now;

            var oldDetails = drugContext.DrugStockInDetail.Where(t => t.ReceiptNo == recepitNo).ToList();

            List<DrugStockInDetail> drugDetail = new List<DrugStockInDetail>();
            foreach (var item in adds)
            {
                var temp = new DrugStockInDetail()
                {
                    DrugId = item.DrugId,
                    StockInNo = stockIn.StockInNo,
                    BatchNo = item.BatchNo,
                    CreateTime = dt,
                    InputQuantity = item.InputQuantity,
                    InQuantity = item.InputQuantity,
                    PurchasePrice = item.PurchasePrice,
                    ReceiptNo = stockIn.ReceiptNo,
                    UnitType = item.UnitType,
                    Id = 0,
                };
                drugDetail.Add(temp);
            }
            drugContext.AddRange(drugDetail);
            drugContext.RemoveRange(oldDetails);
            await drugContext.SaveChangesAsync();
        }
        private async void ModifyPartDrugs(string recepitNo, List<AddStockInDetailVo> adds)
        {
            var stockIn = drugContext.DrugStockIn.SingleOrDefault(t => t.ReceiptNo == recepitNo);
            if (stockIn == null)
            {
                throw new Exception("没有找到该单据信息");
            }
            var drugs = adds.Select(t => t.DrugId);

            var list = drugContext.DrugStockInDetail.Where(t => t.ReceiptNo == stockIn.ReceiptNo && t.StockInNo == stockIn.StockInNo
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
                        StockInNo = stockIn.StockInNo,
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
        /// <summary>
        /// 审核入库单据
        /// </summary>
        /// <param name="recepitNo"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

            DrugStockController.StockIn(drugContext, stockIn);

            stockIn.Status = 1;

            await drugContext.SaveChangesAsync();
        }
        /// <summary>
        /// 删除入库单据
        /// </summary>
        /// <param name="recepitNo"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
        /// <summary>
        /// 获取入库主表数据
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Page<DrugStockInDto>> GetStockInData(DateTime beginDate, DateTime endDate, int pageIndex, int pageSize)
        {
            return await drugContext.DrugStockIn.Where(t => t.CreateTime > beginDate && t.CreateTime < endDate)
                  .SelectByType<DrugStockIn, DrugStockInDto>().ToPageListAsync(new PageInfo() { PageIndex = pageIndex, PageSize = pageSize });
        }
        /// <summary>
        /// 获取入库明细数据
        /// </summary>
        /// <param name="recepitNo"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("{recepitNo}")]
        public async Task<Page<DrugStockInDetailDto>> GetStockDetailData(string recepitNo, int pageIndex, int pageSize)
        {
            return await drugContext.DrugStockInDetail.Where(t => t.ReceiptNo == recepitNo)
                 .SelectByType<DrugStockInDetail, DrugStockInDetailDto>().ToPageListAsync(new PageInfo() { PageIndex = pageIndex, PageSize = pageSize });
        }
    }

    public class QueryDrugDto
    {
        /// <summary>
        /// 0表示中药，1表示西药
        /// </summary>
        public int DrugType { get; set; }
        public string PinYin { get; set; }
        public string DrugName { get; set; }
    }
}
