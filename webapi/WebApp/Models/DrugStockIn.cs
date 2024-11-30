using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    //单据号加特殊标识
    [Comment("药品入库")]
    public class DrugStockIn
    {
        [Comment("入库自增长id编号")]
        [Key]
        public int StockInId { get; set; }
        [Comment("入库编号-生成的编号")]
        public string StockInNo { get; set; }
        [Comment("单据号")]
        [StringLength(50)]
        public string ReceiptNo { get; set; }
        [Comment("验收人")]
        [StringLength(50)]
        public string Checker { get; set; }
        public int CheckerId { get; set; }
        [Comment("创建人")]
        [StringLength(50)]
        public string Creator { get; set; }
        public int CreatorId { get; set; }
        [Comment("审核人")]
        [StringLength(50)]
        public string Approver { get; set; }
        public int ApproverId { get; set; }
        [Comment("备注")]
        public string Remarks { get; set; }
        [Comment("库房编号")]
        public int WarehoueNo { get; set; }
        [Comment("库房名称")]
        public string WarehoueName { get; set; }
        public DateTime CreateTime { get; set; }
        [Comment("0草稿，1提交，审核")]
        public int Status { get; set; }
        [Comment("制造商--这个只需要名字就行了")]
        public string Manufacturer { get; set; }
        [Comment("入库方式")]
        public int InWay { get; set; }
    }

    [Comment("药品入库明细")]
    public class DrugStockInDetail
    {
        public int Id { get; set; }
        [Comment("入库编号")]
        public string StockInNo { get; set; }
        [Comment("单据号")]
        public string ReceiptNo { get; set; }
        [Comment("药品编号")]
        public int DrugId { get; set; }
        [Comment("药品名称")]
        public string DrugName { get; set; }
        [Comment("入库单位类别，0包装单位，1零售单位")]
        public int UnitType { get; set; }
        [Comment("入库数量")]
        public int InQuantity { get; set; }
        [Comment("输入数量")]
        public int InputQuantity { get; set; }

        [Comment("采购金额")]
        public decimal PurchaseAmount { get; set; }
        public DateTime CreateTime { get; set; }
        [Comment("批次号")]
        public string BatchNo { get; set; }
        [Comment("有效期")]
        public DateTime ExpirationDate { get; set; }
    }
}
