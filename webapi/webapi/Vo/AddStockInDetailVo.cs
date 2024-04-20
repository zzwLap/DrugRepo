namespace webapi.Vo
{
    public class AddStockInDetailVo
    {
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public string BatchNo { get; set; }
        public int InputQuantity { get; set; }
        public int UnitType { get; set; }
        public decimal PurchasePrice { get; set; }
    }
}
