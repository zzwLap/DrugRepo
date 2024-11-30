using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    public class DrugWarehouse
    {
        [Key]
        public int StockId { get; set; }
        public string StockNo { get; set; }
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public int WarehouseNo { get; set; }
        public string WarehouseName { get; set; }
        public int ActQuantity { get; set; }
        public int NeedSendQuantity { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class DrugWarehouseDetail
    {
        [Key]
        public int Id { get; set; }
        public string StockNo { get; set; }
        public int DrugId { get; set; }
        public int WarehoseNo { get; set; }
        public string BatchNo { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
