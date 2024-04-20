using Microsoft.EntityFrameworkCore;

namespace webapi.Models
{
    [Comment("入库出库调拨记录")]
    public class DrugInOutInfo
    {
        public int Id { get; set; }
        [Comment("单据号")]
        public string ReceiptNo { get; set; }
        [Comment("0(入库,增益),1(出库,报损),2(调拨)")]
        public int EventType { get; set; }
        [Comment("进出库方式，入库方式具体看入库表，出库方式看出库表")]
        public int InOutWay { get; set; }
        public int DrugId { get; set; }
        public int InOutQuantity { get; set; }
        public int OriginalQuantity { get; set; }
        public int CurrentQuantity { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
