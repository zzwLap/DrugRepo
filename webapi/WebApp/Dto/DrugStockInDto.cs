namespace webapi.Dto
{
    public class DrugStockInDto
    {
        public int StockInId { get; set; }
        public string ReceiptNo { get; set; }
        public string Checker { get; set; }
        public int CheckerId { get; set; }
        public string Creator { get; set; }
        public int CreatorId { get; set; }
        public string Approver { get; set; }
        public int ApproverId { get; set; }
        public string Remarks { get; set; }
        public string StockInType { get; set; }
        public int WarehoueNo { get; set; }
        public string WarehoueName { get; set; }
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }
        public string Manufacturer { get; set; }
        public int InWay { get; set; }
    }
}
