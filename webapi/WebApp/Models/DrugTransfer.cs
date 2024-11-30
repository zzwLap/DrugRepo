using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    [Comment("药品调拨")]
    public class DrugTransfer
    {
        [Key]
        public int TransferId { get; set; }
        [Comment("单据号")]
        public string ReceiptNo { get; set; }
        public int InWarehourseNo { get; set; }
        public string InWarehourseName { get; set; }
        public int OutWarehouseNo { get; set; }
        public string OutWarehouseName { get; set; }
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
        public DateTime CreateTime { get; set; }
    }

    [Comment("药品调拨明细")]
    public class DrugTransferDetail
    {
        public int Id { get; set; }
        [Comment("单据号")]
        public int TransferId { get; set; }
        [Comment("单据号")]
        public string ReceiptNo { get; set; }
        [Comment("药品编号")]
        public int DrugId { get; set; }
        [Comment("入库数量")]
        public int TransferQuantity { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
