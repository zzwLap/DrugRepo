using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class DrugsController(MyContext drugContext) : ControllerBase
    {
        private readonly MyContext drugContext = drugContext;

        [HttpGet]
        public async Task<List<DrugDto>> GetDrugs()
        {
            return await drugContext.Drugs.AsNoTrackingWithIdentityResolution().SelectByType<Drugs, DrugDto>().ToListAsync();
        }

        [HttpGet]
        public async Task<DrugDto?> GetDrug(int drugId)
        {
            return await drugContext.Drugs.AsNoTrackingWithIdentityResolution().SelectByType<Drugs, DrugDto>().FirstOrDefaultAsync(t => t.DrugId == drugId);
        }

        [HttpPost]
        public void AddDrugs(AddDrugVo drugVo)
        {
            drugVo.DrugId = 0;

            var checkDupDrug = drugContext.Drugs.Any(t => t.DrugCode == drugVo.DrugCode);
            if (checkDupDrug)
            {
                throw new Exception("该药品编码已经被使用");
            }
            var drugs = new Drugs();
            drugContext.Drugs.Add(drugs);
            var drug2 = drugContext.Entry(drugs);
            drug2.CurrentValues.SetValues(drugVo);

            drugContext.SaveChanges();
        }

        [HttpPut("{drugId}")]
        public void ModifyPrice(int drugId, ChangeDrugPriceVo drugPrice)
        {
            var drugInfo = drugContext.Drugs.FirstOrDefault(t => t.DrugId == drugPrice.DrugId);
            if (drugInfo == null)
            {
                throw new Exception($"未找到药品编号【{drugPrice.DrugId}】的记录信息");
            }
            drugInfo.PackagePrice = drugPrice.PackagePrice;
            drugPrice.ClinicalPrice = drugPrice.ClinicalPrice;
            //TODO 这里应该记录一下每次修改单价后的记录
            drugContext.SaveChanges();
        }

        [HttpPut("{drugId}")]
        public void ModifyDrugInfo(int drugId, UpdateDrugInfoVo drugInfoVo)
        {
            var drugInfo = drugContext.Drugs.FirstOrDefault(t => t.DrugId == drugInfoVo.DrugId);
            if (drugInfo == null)
            {
                throw new Exception($"未找到药品编号【{drugInfoVo.DrugId}】的记录信息");
            }
            var checkDupDrug = drugContext.Drugs.Any(t => t.DrugCode == drugInfoVo.DrugCode
                            && t.DrugId == drugInfoVo.DrugId);
            if (checkDupDrug)
            {
                throw new Exception("该药品编码已经被使用");
            }
            var drug = drugContext.Drugs.Entry(drugInfo);
            drug.CurrentValues.SetValues(drugInfoVo);
            drugContext.SaveChanges();
        }
    }

    public class ChangeDrugPriceVo
    {
        public int DrugId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "包装单位的销售价要大于或等于0")]
        public decimal PackagePrice { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "临床单位销售价要大于或等于0")]
        public decimal ClinicalPrice { get; set; }
    }

    public class UpdateDrugInfoVo
    {
        [Key]
        public int DrugId { get; set; }
        [Required]
        public string DrugName { get; set; }
        public string Spec { get; set; }
        public string PinYin { get; set; }
        public int Sort { get; set; }
        [Required]
        public string ClinicalUnit { get; set; }
        [Required]
        public string PackageUnit { get; set; }
        //[Range(1, double.MaxValue)]
        //public int C2PQuantity { get; set; }
        public string ApprovalNumber { get; set; }
        [Required]
        public string DrugCode { get; set; }
        public string NationDrugCode { get; set; }
        public string RADManufacturer { get; set; }
        [Required]
        public string DosageForm { get; set; }
        [Required]
        public int DefaultSaleUnit { get; set; }
        [Required]
        public int ClinicalSaleUnit { get; set; }
    }
    public class AddDrugVo : DrugDto
    {
    }

    public class DrugDto
    {
        [Key]
        public int DrugId { get; set; }
        [Required]
        public string DrugName { get; set; }
        public string Spec { get; set; }
        public string PinYin { get; set; }
        public int Sort { get; set; }
        [Required]
        public string ClinicalUnit { get; set; }
        [Required]
        public string PackageUnit { get; set; }
        [Range(1, double.MaxValue)]
        public int C2PQuantity { get; set; }
        public string ApprovalNumber { get; set; }
        [Required]
        public string DrugCode { get; set; }
        public string NationDrugCode { get; set; }
        public string RADManufacturer { get; set; }
        [Required]
        public string DosageForm { get; set; }
        [Required]
        public int DefaultSaleUnit { get; set; }
        [Required]
        public int ClinicalSaleUnit { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "包装单位的销售价要大于或等于0")]
        public decimal PackagePrice { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "临床单位销售价要大于或等于0")]
        public decimal ClinicalPrice { get; set; }
    }
}
