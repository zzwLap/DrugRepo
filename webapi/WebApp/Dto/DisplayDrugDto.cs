namespace webapi.Dto
{
    public class DisplayDrugDto
    {
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public string Spec { get; set; }
        public string PinYin { get; set; }
        public string ClinicalUnit { get; set; }
        public string PackageUnit { get; set; }
        public int C2PQuantity { get; set; }
        public string ApprovalNumber { get; set; }
        public string DrugCode { get; set; }
        public string DosageForm { get; set; }
        public decimal PackagePrice { get; set; }
    }
}
