using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    [Comment("药品出库")]
    public class DrugStockOut
    {
        [Comment("出库主键")]
        [Key]
        public int StockOutId { get; set; }
        [Comment("出库编号")]
        public int StockOutNo { get; set; }
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
        [Comment("出库类别")]
        public string StockOutType { get; set; }
        [Comment("库房编号")]
        public string WarehoueNo { get; set; }
        [Comment("库房名称")]
        public string WarehoueName { get; set; }
        public DateTime CreateTime { get; set; }
        [Comment("出库方式")]
        public int OutWay { get; set; }
    }

    [Comment("药品出库明细")]
    public class DrugStockOutDetail
    {
        public int Id { get; set; }
        [Comment("单据号")]
        public int StockOutId { get; set; }
        [Comment("单据号")]
        public string ReceiptNo { get; set; }
        [Comment("药品编号")]
        public int DrugId { get; set; }
        [Comment("入库数量")]
        public int InQuantity { get; set; }
        [Comment("出库金额")]
        public decimal StockOutAmount { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
