using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    public class Drugs
    {
        [Key]
        public int DrugId { get; set; }
        [MaxLength(50)]
        [Comment("药品名称")]
        public string DrugName { get; set; }
        [Comment("药品规格")]
        public string Spec { get; set; }
        [Comment("拼音码")]
        public string PinYin { get; set; }
        [Comment("排序编号")]
        public int Sort { get; set; }
        [Comment("临床单位")]
        public string ClinicalUnit { get; set; }
        [Comment("包装单位")]
        public string PackageUnit { get; set; }
        [Comment("临床到包装单位的转换")]
        public int C2PQuantity { get; set; }
        [Comment("批准文号")]
        public string ApprovalNumber { get; set; }
        [Comment("本地药品编码")]
        public string DrugCode { get; set; }
        [Comment("国家药品编码")]
        public string NationDrugCode { get; set; }
        [Comment("药品研发厂家")]
        public string RADManufacturer { get; set; }
        //[Comment("生产厂家")]
        //public string Manufacturer { get; set;  
        [Comment("药品剂型")]
        public string DosageForm { get; set; }

        [Comment("销售单位")]
        public int DefaultSaleUnit { get; set; }
        [Comment("临床销售单位")]
        public int ClinicalSaleUnit { get; set; }

        [Comment("包装单位销售单价")]
        [Precision(16, 4)]
        public decimal PackagePrice { get; set; }
        [Comment("临床销售单价")]
        [Precision(16, 4)]
        public decimal ClinicalPrice { get; set; }
    }
}
