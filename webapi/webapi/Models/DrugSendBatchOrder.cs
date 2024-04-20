using Microsoft.EntityFrameworkCore;

namespace webapi.Models
{
    [Comment("执行批次顺序表")]
    public class DrugSendBatchOrder
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int DrugId { get; set; }
        public string ReceiptNo { get; set; }
        public string BatchNo { get; set; }
        public int Quantity { get; set; }
        [Comment("入库,出库,调拨，增益，报损")]
        //TODO (入库，增益) (出库，报损) 是否可以合并在一起
        public int EventType { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
