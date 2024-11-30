namespace webapi.Dto
{
    public class DrugStockInDetailDto
    {
        public int Id { get; set; }
        public string ReceiptNo { get; set; }
        public int DrugId { get; set; }
        public int DrugName { get; set; }
        public int UnitType { get; set; }
        public int InputQuantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime CreateTime { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
